using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityHFSM;

public class MachineDancerHFSMStateManager : MonoBehaviour
{
    #region COMPONENTS
    [Header("Component")]
    private Rigidbody2D rb;
    public Collider2D MyselfCollider;
    private StateMachine fsm;
    private Animator animator;


    public GameObject wavePrefab;
    #endregion

    [Header("Checksbox")]
    public Transform pivotPoint;
    public Vector2 L_firePoint_Offset;
    public Vector2 R_firePoint_Offset;
    [Space(10)]

    [Header("Status")]
    public bool isFacingRight = true;
    public bool isSlashing;
    public bool isWaiting;
    [Space(10)]

    [Header("Adjustment")]
    public float force = 10f;
    float betweenWaves = 3f;
    #region WALK
    public Vector2 groundCheckOffset;
    // public Vector2 groundCheckSize;
    public float xGroundCheck, yGroundCheck;
    public Vector2 R_WallCheckOffset;
    public Vector2 R_WallCheckSize;
    public Vector2 L_WallCheckOffset;
    public Vector2 L_WallCheckSize;
    public float walkSpeed = 2f;
    public float walkSec = 2f;
    public float walkTimer;


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
        fsm.AddState("Patrol", onEnter: state => { walkTimer = walkSec; Debug.Log("巡邏進入 " + walkTimer); animator.SetBool("patrol", true); },
             onLogic: state =>
             {
                 Debug.Log("巡邏中");
                 walkTimer -= Time.deltaTime;
                 rb.velocity = new Vector2(walkSpeed * (isFacingRight ? 1 : -1), rb.velocity.y);
                 if (isFacingRight && R_WallCheck())
                 {
                     Turn();
                 }
                 else if (!isFacingRight && L_WallCheck())
                 {
                     Turn();
                 }
                 else if (!GroundCheck())
                 {
                     Turn();
                     rb.velocity = new Vector2(0, rb.velocity.y);
                 }

             }, onExit: state => animator.SetBool("patrol", false));
        fsm.AddState("Slash", new CoState(this, FeetSlash, loop: false, canExit: state => !isSlashing, needsExitTime: true));

        fsm.AddState("Wait", onEnter: state => { animator.SetBool("slash", false); animator.SetBool("wait", true); WaitForAttack(); rb.velocity = new Vector2(0, rb.velocity.y); }, onExit: state => { Turn(); animator.SetBool("wait", false); }, canExit: state => !isWaiting, needsExitTime: true);
        fsm.AddTransition("Patrol", "Slash", t => walkTimer <= 0);
        fsm.AddTransition("Slash", "Wait");
        fsm.AddTransition("Wait", "Patrol");


        fsm.SetStartState("Patrol");
        fsm.Init();
    }
    void Update()
    {
        fsm.OnLogic();
    }
    private IEnumerator FeetSlash()
    {
        int AttackRound = 1;
        isSlashing = true;
        animator.SetBool("slash", true);
        while (AttackRound > 0)
        {


            GameObject Wave1;
            GameObject Wave2;
            GameObject Wave3;
            if (isFacingRight)
            {
                Wave1 = Instantiate(wavePrefab, (Vector2)pivotPoint.position + R_firePoint_Offset, quaternion.identity);
                Wave2 = Instantiate(wavePrefab, (Vector2)pivotPoint.position + R_firePoint_Offset + new Vector2(1.5f, 0), quaternion.identity);
                Wave3 = Instantiate(wavePrefab, (Vector2)pivotPoint.position + R_firePoint_Offset + new Vector2(3f, 0), quaternion.identity);
            }
            else
            {
                Wave1 = Instantiate(wavePrefab, (Vector2)pivotPoint.position + L_firePoint_Offset, quaternion.identity);
                Wave2 = Instantiate(wavePrefab, (Vector2)pivotPoint.position + L_firePoint_Offset + new Vector2(-1.5f, 0), quaternion.identity);
                Wave3 = Instantiate(wavePrefab, (Vector2)pivotPoint.position + L_firePoint_Offset + new Vector2(-3f, 0), quaternion.identity);
            }

            Rigidbody2D wave1 = Wave1.GetComponent<Rigidbody2D>();
            Rigidbody2D wave2 = Wave2.GetComponent<Rigidbody2D>();
            Rigidbody2D wave3 = Wave3.GetComponent<Rigidbody2D>();
            Vector3 scale = transform.localScale;

            wave1.AddForce(new Vector2(scale.x, 0) * force, ForceMode2D.Impulse);
            wave2.AddForce(new Vector2(scale.x, 0) * force, ForceMode2D.Impulse);
            wave3.AddForce(new Vector2(scale.x, 0) * force, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.5f);
            AttackRound--;
        }
        isSlashing = false;
        // animator.SetBool("slash", false);

        yield break;
    }
    private void WaitForAttack()
    {
        isWaiting = true;
        Invoke("UnWait", betweenWaves);
    }
    private void UnWait()
    {
        isWaiting = false;
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
    public bool GroundCheck()
    {
        if (isFacingRight)
        {
            if (Physics2D.Raycast((Vector2)pivotPoint.position + groundCheckOffset, Vector2.down, yGroundCheck, groundLayer) &&
            Physics2D.Raycast((Vector2)pivotPoint.position + groundCheckOffset + new Vector2(xGroundCheck, 0), Vector2.down, yGroundCheck, groundLayer))
                return true;
            else
                return false;
        }
        else
        {
            if (Physics2D.Raycast((Vector2)pivotPoint.position + groundCheckOffset, Vector2.down, yGroundCheck, groundLayer) &&
            Physics2D.Raycast((Vector2)pivotPoint.position + groundCheckOffset + new Vector2(-xGroundCheck, 0), Vector2.down, yGroundCheck, groundLayer))
                return true;
            else
                return false;
        }

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
        Gizmos.DrawWireCube((Vector2)pivotPoint.position + R_firePoint_Offset, new Vector2(0.2f, 0.2f));
        Gizmos.DrawWireCube((Vector2)pivotPoint.position + L_firePoint_Offset, new Vector2(0.2f, 0.2f));


    }
}
