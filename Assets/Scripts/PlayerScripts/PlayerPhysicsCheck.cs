using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsCheck : MonoBehaviour
{
    #region STATE VARIABLES
    [SerializeField]
    private PlayerAttribute player_attribute;
    #endregion

    #region COMPONENTS
    public Rigidbody2D RB { get; private set; }
    public Collider2D MyselfCollider { get; private set; }

    #endregion

    #region CHECK PARAMETERS
    //Set all of these up in the inspector
    [Header("Checks")]
    [SerializeField] private Transform ground_checkpoint;
    //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
    [SerializeField] private Vector2 ground_checkSize = new Vector2(0.9f, 0.06f);
    [SerializeField] private Transform roof_checkpoint;
    [SerializeField] private Vector2 roof_checkSize = new Vector2(0.9f, 0.06f);
    [Space(5)]
    [SerializeField] private Transform punch_point;
    [SerializeField] private float punch_radius = 1f;

    [Space(5)]

    #endregion

    #region LAYERS
    [Header("Layers")]
    [SerializeField] private LayerMask ground_layer;
    [SerializeField] private LayerMask attackable_layer;

    #endregion

    #region TIMERS
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallTime { get; private set; }

    #endregion

    #region OTHER VARIABLES
    public bool IsFacingRight { get; private set; }
    public int FacingDirection { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }

    #endregion

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        MyselfCollider = GetComponent<Collider2D>();
        IsFacingRight = true;
        FacingDirection = 1;
    }
    private void Update()
    {
        CurrentVelocity = RB.velocity;
        FacingDirection = (int)transform.localScale.x;

        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;

    }

    #region TIMER METHODS
    public void SetLastOnGroundTimeToZero()
    {
        LastOnGroundTime = 0;
    }
    public void SetLastOnWallRightTimeToZero()
    {
        LastOnWallRightTime = 0;
    }

    public void SetLastOnWallLeftTimeToZero()
    {
        LastOnWallLeftTime = 0;
    }

    #endregion

    #region CHECK METHODS

    #region GROUND METHOD
    public bool CheckIfGrounded()
    {
        return LastOnGroundTime > 0;
    }
    public void OnGroundCheck()
    {
        if (Physics2D.OverlapBox(ground_checkpoint.position, ground_checkSize, 0, ground_layer)) //checks if set box overlaps with ground
        {
            //Debug.Log("ground");
            //if so sets the lastGrounded to coyoteTime  coyoteTime:當玩家自地形邊界走出，發生離地的瞬間，此時角色已經底部浮空，但玩家仍可以進行跳躍的指令。
            LastOnGroundTime = player_attribute.CoyoteTime;
        }
    }
    #region PUNCH ATTACK METHOD

    public List<Collider2D> CheckHittedUnit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(punch_point.position, punch_radius, attackable_layer);
        List<Collider2D> hitted_enemies = new List<Collider2D>();

        foreach (Collider2D Enemy in hitEnemies)
        {
            // Debug.Log("撞到那些人: " + Enemy.name);
            hitted_enemies.Add(Enemy);
            if (Enemy == MyselfCollider)
                hitted_enemies.Remove(Enemy);
        }
        return hitted_enemies;
    }
    #endregion


    #endregion

    #region SLOPE

    #endregion


    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }

    public void Turn()
    {
        //stores scale and flips the player along the x axis, 

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        IsFacingRight = !IsFacingRight;

        /*
		if(!IsDashing)
		{
			Vector3 scale = transform.localScale; 
			scale.x *= -1;
			transform.localScale = scale;

			IsFacingRight = !IsFacingRight;
		}
        */
    }

    #endregion

    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(ground_checkpoint.position, ground_checkSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(roof_checkpoint.position, roof_checkSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(punch_point.position, punch_radius);

        //Gizmos.color=Color.white;
        //Gizmos.DrawSphere(_slashPoint.position,_slashRadius);//3Dball WTF!!
        //Gizmos.DrawWireCube(_barrelCheckPoint.position, _pushCheckSize);
        //Gizmos.DrawRay(_groundCheckPoint.position , Vector2.down * slopeRaycastDistance);
    }
    #endregion
}