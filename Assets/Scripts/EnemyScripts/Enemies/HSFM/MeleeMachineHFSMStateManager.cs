using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;
using UnityHFSM;

public class MeleeMachineHFSMStateManager : MonoBehaviour
{
    #region COMPONENTS
    [Header("Component")]
    private Rigidbody2D rb;
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
    #region WALK
    public Vector2 groundCheckOffset;
    public Vector2 groundCheckSize;
    public float xGroundCheck, yGroundCheck;
    public Vector2 R_WallCheckOffset;
    public Vector2 R_WallCheckSize;
    public Vector2 L_WallCheckOffset;
    public Vector2 L_WallCheckSize;
    public float walkSpeed = 10f;
    public Transform playerPos;//chage to private
    #endregion

    #region  LAYER
    public LayerMask GroundLayer;
    public LayerMask AttackLayer;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        fsm = new StateMachine();
        fsm.AddState("walk", onEnter: state => animator.Play("walk"),
            onLogic: state =>
            {
                // rb.velocity = new Vector2(walkSpeed * (isFacingRight ? -1 : 1), rb.velocity.y);
                rb.velocity = new Vector2(walkSpeed * (isFacingRight ? 1 : -1), rb.velocity.y);
                if (isFacingRight && R_WallCheck())
                    Turn();
                else if (!isFacingRight && L_WallCheck())
                    Turn();
            });
        fsm.AddState("fall", onEnter: state => animator.Play("fall"));
        fsm.AddTransition("walk", "fall", t => rb.velocity.y < 0);
        fsm.AddTransition("fall", "walk", t => rb.velocity.y >= 0);
        fsm.SetStartState("walk");
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
        if (Physics2D.OverlapBox((Vector2)pivotPoint.position + R_WallCheckOffset, R_WallCheckSize, 0, GroundLayer))
            return true;
        else
            return false;
    }
    public bool L_WallCheck()
    {
        if (Physics2D.OverlapBox((Vector2)pivotPoint.position + L_WallCheckOffset, L_WallCheckSize, 0, GroundLayer))
            return true;
        else
            return false;
    }
    public bool GroundCheck()
    {
        if (Physics2D.Raycast((Vector2)pivotPoint.position + groundCheckOffset, Vector2.down, yGroundCheck, GroundLayer))
            return true;
        else
            return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine((Vector2)pivotPoint.position + groundCheckOffset, (Vector2)pivotPoint.position + groundCheckOffset + new Vector2(0, -yGroundCheck));
        Gizmos.DrawLine((Vector2)pivotPoint.position + groundCheckOffset + new Vector2(xGroundCheck, 0), (Vector2)pivotPoint.position + groundCheckOffset + new Vector2(xGroundCheck, -yGroundCheck));
        Gizmos.DrawLine((Vector2)pivotPoint.position + groundCheckOffset + new Vector2(-xGroundCheck, 0), (Vector2)pivotPoint.position + groundCheckOffset + new Vector2(-xGroundCheck, -yGroundCheck));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)pivotPoint.position + R_WallCheckOffset, R_WallCheckSize);
        Gizmos.DrawWireCube((Vector2)pivotPoint.position + L_WallCheckOffset, L_WallCheckSize);

    }
}
