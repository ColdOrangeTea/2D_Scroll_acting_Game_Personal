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
    public float walkSpeed = 10f;
    public Transform playerPos;//chage to private
    #endregion
    public LayerMask GroundLayer;

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
                if(!GroundCheck())
                {
                    Turn();
                }
            });
        fsm.SetStartState("walk");
        fsm.Init();
    }
    void Update()
    {
        fsm.OnLogic();
    }

    //public void FacingPlayer()
    //{
    //    if (playerPos.position.x < transform.position.x != isFacingRight)
    //        Turn();
    //}
    public void Turn()
    {
        //stores scale and flips the enemy along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }

    public bool GroundCheck()
    {
        if(Physics2D.Raycast((Vector2)pivotPoint.position + groundCheckOffset, Vector2.down, yGroundCheck,GroundLayer))
            return true;
        else
            return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine((Vector2)pivotPoint.position + groundCheckOffset,(Vector2)pivotPoint.position + groundCheckOffset+new Vector2(0,-yGroundCheck)) ;
        Gizmos.DrawLine((Vector2)pivotPoint.position + groundCheckOffset+new Vector2(xGroundCheck,0),(Vector2)pivotPoint.position + groundCheckOffset+new Vector2(xGroundCheck, -yGroundCheck)) ;
        Gizmos.DrawLine((Vector2)pivotPoint.position + groundCheckOffset + new Vector2(-xGroundCheck,0), (Vector2)pivotPoint.position + groundCheckOffset + new Vector2(-xGroundCheck, -yGroundCheck));

    }
}
