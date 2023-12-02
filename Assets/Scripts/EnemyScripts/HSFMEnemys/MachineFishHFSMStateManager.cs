using UnityEngine;
using UnityHFSM;

public class MachineFishHFSMStateManager : MonoBehaviour
{
    #region COMPONENTS
    [Header("Component")]
    private Rigidbody2D rb;
    public Collider2D MyselfCollider;
    private StateMachine fsm;
    private Animator animator;
    #endregion

    [Header("Checksbox")]
    public Transform pivotPoint;
    [Space(10)]

    [Header("Status")]
    public bool isFacingRight = true;
    [Space(10)]

    [Header("Adjustment")]
    #region FLY
    public Vector2 R_WallCheckOffset;
    public Vector2 R_WallCheckSize;
    public Vector2 L_WallCheckOffset;
    public Vector2 L_WallCheckSize;
    public float walkSpeed = 2f;
    // public Transform playerPos;//chage to private
    #endregion

    #region --LAYERS--
    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private LayerMask thingLayer;
    enum NumOfLayer
    {
        AttackableUnit = 7,
        Thing = 8,
        Attack = 9
    }
    #endregion

    #region TAG NAME
    public const string PLAYER = "Player";
    public const string B_THING = "B_Thing";
    public const string P_THING = "P_Thing";
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MyselfCollider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        fsm = new StateMachine();
        fsm.AddState("fly",// onEnter: state => animator.Play("fly"),
            onLogic: state =>
            {
                rb.velocity = new Vector2(walkSpeed * (isFacingRight ? 1 : -1), rb.velocity.y);
                if (isFacingRight && R_WallCheck())
                {
                    Turn();
                }
                else if (!isFacingRight && L_WallCheck())
                {
                    Turn();
                }
            });
        fsm.SetStartState("fly");
        fsm.Init();
    }
    void Update()
    {
        fsm.OnLogic();
    }

    public void Turn()
    {
        //stores scale and flips the enemy along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }
    public bool R_WallCheck()
    {
        // if (isFacingRight)
        // {
        //     if (Physics2D.OverlapBox((Vector2)pivotPoint.position + R_WallCheckOffset, R_WallCheckSize, 0, groundLayer) ||
        //     Physics2D.OverlapBox((Vector2)pivotPoint.position + R_WallCheckOffset, R_WallCheckSize, 0, thingLayer).CompareTag(B_THING))
        //         return true;
        // }
        // return false;
        if (isFacingRight)
        {
            if (Physics2D.OverlapBox((Vector2)pivotPoint.position + R_WallCheckOffset, R_WallCheckSize, 0, thingLayer))
            {
                Collider2D wall = Physics2D.OverlapBox((Vector2)pivotPoint.position + R_WallCheckOffset, R_WallCheckSize, 0, thingLayer);
                if (wall.CompareTag(B_THING) || wall.CompareTag(P_THING))
                    return true;
            }
            else if (Physics2D.OverlapBox((Vector2)pivotPoint.position + R_WallCheckOffset, R_WallCheckSize, 0, groundLayer))
            {
                return true;
            }
        }
        return false;

    }
    public bool L_WallCheck()
    {
        if (!isFacingRight)
        {
            if (Physics2D.OverlapBox((Vector2)pivotPoint.position + L_WallCheckOffset, L_WallCheckSize, 0, thingLayer))
            {
                Collider2D wall = Physics2D.OverlapBox((Vector2)pivotPoint.position + L_WallCheckOffset, L_WallCheckSize, 0, thingLayer);
                if (wall.CompareTag(B_THING) || wall.CompareTag(P_THING))
                    return true;
            }
            else if (Physics2D.OverlapBox((Vector2)pivotPoint.position + L_WallCheckOffset, L_WallCheckSize, 0, groundLayer))
            {
                return true;
            }

        }
        return false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)pivotPoint.position + R_WallCheckOffset, R_WallCheckSize);
        Gizmos.DrawWireCube((Vector2)pivotPoint.position + L_WallCheckOffset, L_WallCheckSize);

    }
}
