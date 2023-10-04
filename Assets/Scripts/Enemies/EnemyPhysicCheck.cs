using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhysicCheck : MonoBehaviour
{

    #region COMPONENTS
    public Animator Anim { get; private set; }
    // public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }

    public Collider2D Myself { get; private set; }

    #endregion
    #region CHECK PARAMETERS
    //Set all of these up in the inspector
    [Header("Checks")]
    [SerializeField] private Transform ground_checkpoint;

    //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
    [SerializeField] private Vector2 ground_checkSize = new Vector2(1.8f, 0.06f);
    [Space(5)]
    [SerializeField] private Transform roof_checkpoint;
    [SerializeField] private Vector2 roof_checkSize = new Vector2(1.8f, 0.06f);
    [Space(5)]

    [SerializeField] private Transform wall_checkpoint;
    [SerializeField] private Vector2 wall_checkSize = new Vector2(2, 3);
    [Space(5)]

    [SerializeField] private Transform player_checkpoint;
    [SerializeField] private Vector2 player_checkSize = new Vector2(2, 2);
    [Space(5)]

    [SerializeField] private Transform melee_attack_point;
    [SerializeField] private float melee_attack_radius = 1.5f;
    [Space(5)]


    #endregion

    #region LAYERS
    [Header("Layers")]
    [SerializeField] private LayerMask ground_layer;
    [SerializeField] private LayerMask attackable_layer;

    #endregion

    #region TIMERS
    public float LastOnGroundTime { get; private set; }

    #endregion

    #region OTHER VARIABLES
    public bool IsFacingRight { get; private set; }
    public int FacingDirection { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }

    #endregion
    void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        RB = GetComponent<Rigidbody2D>();
        Myself = GetComponent<Collider2D>();

        IsFacingRight = true;
        FacingDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentVelocity = RB.velocity;
        FacingDirection = (int)transform.localScale.x;

        LastOnGroundTime -= Time.deltaTime;
    }

    #region --CHECK METHODS--

    #region GROUND METHOD
    public bool CheckIfGrounded()
    {
        if (Physics2D.OverlapBox(ground_checkpoint.position, ground_checkSize, 0, ground_layer)) //checks if set box overlaps with ground
            return true;
        else
            return false;
    }

    #endregion

    #region ROOFED METHOD
    public bool CheckIfRoofed()
    {
        if (Physics2D.OverlapBox(roof_checkpoint.position, roof_checkSize, 0, ground_layer)) //checks if set box overlaps with ground
            return true;
        else
            return false;
    }
    #endregion

    #region TOUCHINGWALL METHOD
    public bool CheckIfTouchingWall()
    {
        if (Physics2D.OverlapBox(wall_checkpoint.position, wall_checkSize, 0, ground_layer)) //checks if set box overlaps with ground
            return true;
        else
            return false;
    }
    #endregion

    #region PLAYERCHECK METHOD
    public bool CheckIfSawPlayer()
    {
        if (Physics2D.OverlapBox(player_checkpoint.position, player_checkSize, 0, attackable_layer)) //checks if set box overlaps with ground
        {
            if (Physics2D.OverlapBox(player_checkpoint.position, player_checkSize, 0, attackable_layer) == Myself)
            {
                return false;
            }
            else
            {
                if (Physics2D.OverlapBox(player_checkpoint.position, player_checkSize, 0, attackable_layer).CompareTag("Player"))
                {
                    return true;
                }
                else // 不是自己 也不是玩家
                    return false;
            }
        }
        else // 沒東西
            return false;

    }
    #endregion

    #region MELEE ATTACK METHOD

    public List<Collider2D> CheckHittedUnit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(melee_attack_point.position, melee_attack_radius, attackable_layer);
        List<Collider2D> hitted_enemies = new List<Collider2D>();

        foreach (Collider2D Enemy in hitEnemies)
        {
            // Debug.Log("撞到那些人: " + Enemy.name);
            hitted_enemies.Add(Enemy);
            if (Enemy == Myself)
                hitted_enemies.Remove(Enemy);
        }
        return hitted_enemies;
    }
    #endregion


    #region SLOPE
    #endregion

    #region TURN AROUND METHOD
    public bool CheckIfNeedToTurn(bool isMovingRight)
    {
        if (CheckIfTouchingWall())
        {
            Debug.Log("碰牆要轉向 ");
            return true;
        }
        else if (!CheckIfGrounded())
        {
            Debug.Log("前方沒地板要轉向 ");
            return true;
        }
        else
        {
            return false;
        }

    }
    #endregion


    #endregion

    #region --EDITOR METHODS--
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(ground_checkpoint.position, ground_checkSize);
        Gizmos.DrawWireCube(roof_checkpoint.position, roof_checkSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wall_checkpoint.position, wall_checkSize);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(player_checkpoint.position, player_checkSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(melee_attack_point.position, melee_attack_radius);

        //Gizmos.color=Color.white;
        //Gizmos.DrawSphere(_slashPoint.position,_slashRadius);//3Dball WTF!!
        //Gizmos.DrawWireCube(_barrelCheckPoint.position, _pushCheckSize);
        //Gizmos.DrawRay(_groundCheckPoint.position , Vector2.down * slopeRaycastDistance);
    }
    #endregion

}
