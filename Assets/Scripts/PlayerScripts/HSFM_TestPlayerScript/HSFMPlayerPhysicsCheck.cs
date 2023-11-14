using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSFMPlayerPhysicsCheck : MonoBehaviour
{
    #region --COMPONENTS--
    [SerializeField]
    private PlayerHFSMStateManager player;
    [SerializeField]
    private HSFMPlayerInputHandler inputHandler;
    [SerializeField]
    private PlayerAttribute attribute;
    public Rigidbody2D RB { get; private set; }
    public Collider2D OwnCollider { get; private set; }
    #endregion

    #region --CHECK PARAMETERS--
    //Set all of these up in the inspector
    [Header("Checks")]
    [SerializeField] private Vector2 ground_check_offset;
    [SerializeField] private Transform ground_checkpoint;
    //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
    [SerializeField] private Vector2 ground_checkSize = new Vector2(0.9f, 0.06f);
    [Space(5)]

    [SerializeField] private Vector2 roof_check_offset;
    [SerializeField] private Transform roof_checkpoint;
    [SerializeField] private Vector2 roof_checkSize = new Vector2(0.9f, 0.06f);
    [Space(5)]

    [SerializeField] private Vector2 punch_check_offset;
    [SerializeField] private Transform punch_checkpoint;
    [SerializeField] private float punch_radius = 1f;
    [Space(5)]
    #endregion

    #region --LAYERS--
    [Header("Layers")]
    [SerializeField] private LayerMask ground_layer;
    [SerializeField] private LayerMask attackable_layer;
    [SerializeField] private LayerMask thing_layer;
    enum NumOfLayer
    {
        AttackableUnit = 7,
        Thing = 8,
        Attack = 9
    }
    #endregion

    #region --TIMERS--
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallTime { get; private set; }

    #endregion

    #region OTHER VARIABLES
    public bool IsFacingRight { get; private set; }
    public int FacingDirection { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }
    public bool onGround { get; private set; }
    #endregion   

    #region TAG NAME
    public const string ENEMY = "Enemy";
    public const string BULLET = "Bullet";
    public const string B_THING = "B_Thing";
    public const string I_THING = "I_Thing";
    public const string P_THING = "P_Thing";
    #endregion

    public void GetPlayerAttribute(object source, UnitAttributeEventArgs args)
    {
        this.player = args.Player;
        this.inputHandler = args.Player.InputHandler;
        this.attribute = args.Player.Attribute;
    }
    #region UNITY CALLBACK FUNCTIONS
    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        OwnCollider = GetComponent<Collider2D>();
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
    #endregion

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
        if (Physics2D.OverlapBox((Vector2)ground_checkpoint.position + ground_check_offset, ground_checkSize, 0, ground_layer)) //checks if set box overlaps with ground
        {
            //if so sets the lastGrounded to coyoteTime  coyoteTime:當玩家自地形邊界走出，發生離地的瞬間，此時角色已經底部浮空，但玩家仍可以進行跳躍的指令。
            LastOnGroundTime = attribute.CoyoteTime;
            onGround = CheckIfGrounded();
        }
        else
        {
            onGround = CheckIfGrounded();
        }
        // Debug.Log("OnGroundCheck: " + onGround);
    }
    #endregion
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == (int)NumOfLayer.AttackableUnit)
        {
            if (other.gameObject.CompareTag(ENEMY))
            {
                Debug.Log("Collision Enemy");
                EnemyStatus status = other.gameObject.GetComponent<EnemyStatus>();
                player.TakeColliderDamage(status);
            }
        }
        if (other.gameObject.layer == (int)NumOfLayer.Attack)
        {
            if (other.gameObject.CompareTag(BULLET))
            {
                Debug.Log("Collision Bullet");
                Bullet bullet = other.gameObject.GetComponent<Bullet>();
                player.TakeColliderDamage(bullet);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == (int)NumOfLayer.Thing)
        {
            if (other.gameObject.CompareTag(P_THING))
            {
                Debug.Log("Trigger P_THING");
                other.gameObject.GetComponent<Thing>().TriggerThing();
            }
        }
    }

    #region GAIN THINGS METHOD
    public List<Collider2D> CheckHittedThing()
    {
        Collider2D[] hit_things = Physics2D.OverlapCircleAll((Vector2)punch_checkpoint.position + punch_check_offset, punch_radius, thing_layer);
        List<Collider2D> hitted_things = new List<Collider2D>();
        Debug.Log("有沒有擊中的物件: ");
        foreach (Collider2D hitted_thing in hit_things)
        {
            Debug.Log("擊中的物件是: " + hitted_thing.name);
            hitted_things.Add(hitted_thing);
        }
        return hitted_things;
    }
    #endregion

    #region PUNCH ATTACK METHOD
    public List<Collider2D> CheckHittedUnit()
    {
        Collider2D[] hit_enemies = Physics2D.OverlapCircleAll((Vector2)punch_checkpoint.position + punch_check_offset, punch_radius, attackable_layer);
        List<Collider2D> hitted_units = new List<Collider2D>();

        foreach (Collider2D hitted_unit in hit_enemies)
        {
            hitted_units.Add(hitted_unit);
            if (hitted_unit == OwnCollider)
            {
                hitted_units.Remove(hitted_unit);
            }
            // Debug.Log("撞到那些人: " + hitted_unit.name);
        }
        return hitted_units;
    }
    #endregion




    #region SLOPE

    #endregion

    public void CheckDirectionToFace_Test()
    {
        bool isMovingRight = inputHandler.XInput > 0;

        if (inputHandler.XInput == 0)
        {

        }
        else if (isMovingRight != IsFacingRight)
            Turn();
        // bool isMovingRight = inputHandler.XInput > 0;
        // if (isMovingRight != IsFacingRight)
        //     Turn();
    }
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
        Gizmos.DrawWireCube((Vector2)ground_checkpoint.position + ground_check_offset, ground_checkSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)roof_checkpoint.position + roof_check_offset, roof_checkSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)punch_checkpoint.position + punch_check_offset, punch_radius);

        //Gizmos.color=Color.white;
        //Gizmos.DrawSphere(_slashPoint.position,_slashRadius);//3Dball WTF!!
        //Gizmos.DrawWireCube(_barrelCheckPoint.position, _pushCheckSize);
        //Gizmos.DrawRay(_groundCheckPoint.position , Vector2.down * slopeRaycastDistance);
    }
    #endregion
}
