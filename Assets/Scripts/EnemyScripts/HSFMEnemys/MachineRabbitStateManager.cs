using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityHFSM;
public class MachineRabbitStateManager : MonoBehaviour
{
    private Rigidbody2D rb;
    public Collider2D MyselfCollider;
    private UnityHFSM.StateMachine fsm;
    // private Animator animator;
    private Text stateDisplayText;
    public Transform playerPos;//chage to private

    public Transform firePoint;
    public Transform Canon1;
    public Transform Canon2;
    public Transform Canon3;
    public GameObject axePrefab;


    [Header("Adjustment")]
    // public float chaseSpeed = 10f;
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
    // public Transform playerPos;//chage to private
    #endregion


    [Header("Checksbox")]
    public Transform pivotPoint;
    public Vector2 shootRangePointOffset;
    public float rangedRadius = 1f;


    [Header("Status")]
    public bool isFacingRight = true;
    public bool inShootRange;
    public bool isThrowing;
    public bool isWaiting;

    #region --LAYERS--
    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask AttackableUnitLayer;
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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MyselfCollider = GetComponent<Collider2D>();
        // animator = GetComponentInChildren<Animator>();
        stateDisplayText = GetComponentInChildren<Text>();
        fsm = new UnityHFSM.StateMachine();
        fsm.AddState("Patrol", //onEnter: state => ,animator.Play("Walk")
        onLogic: state =>
        {
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
                facingPlayer();
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else if (GroundCheck())
            {
                facingPlayer();
                rb.velocity = new Vector2(walkSpeed * (isFacingRight ? 1 : -1), rb.velocity.y);
            }


        });
        fsm.AddState("Throw", new CoState(this, ThreeWaveAxeThrow, loop: false, canExit: state => !isThrowing, needsExitTime: true));
        fsm.AddState("Wait", onEnter: state => { WaitForAttack(); rb.velocity = new Vector2(0, rb.velocity.y); }, canExit: state => !isWaiting, needsExitTime: true);
        fsm.AddTransition("Patrol", "Throw", t => inShootRange == true);
        fsm.AddTransition("Throw", "Wait");
        fsm.AddTransition("Wait", "Patrol");


        fsm.SetStartState("Patrol");
        fsm.Init();
    }

    // Update is called once per frame
    void Update()
    {
        fsm.OnLogic();
        stateDisplayText.text = fsm.GetActiveHierarchyPath();

        if (Physics2D.OverlapCircle((Vector2)pivotPoint.position + shootRangePointOffset, rangedRadius, AttackableUnitLayer))
        {
            Collider2D hittedUnit = Physics2D.OverlapCircle((Vector2)pivotPoint.position + shootRangePointOffset, rangedRadius, AttackableUnitLayer);
            // Debug.Log("偵測物件中 ");
            if (hittedUnit.CompareTag("Player"))
            {
                // Debug.Log("玩家偵測 ");
                inShootRange = true;
            }
            else
            {
                // Debug.Log("玩家沒有偵測到 ");
                inShootRange = false;
            }

        }
    }
    public void facingPlayer()
    {
        if (playerPos.position.x < transform.position.x == isFacingRight)
        {
            Turn();
        }
        // if (playerPos.position.x < transform.position.x != isFacingRight)
        // {
        //     Debug.Log("轉身 ");
        //     Turn();
        // }

    }
    public void Turn()
    {
        //stores scale and flips the enemy along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }
    // bool AnyAnimatorIsPlaying()
    // {
    //     return animator.GetCurrentAnimatorStateInfo(0).length >
    //            animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    // }
    // bool AnimatorIsPlaying(string stateName)
    // {
    //     return AnyAnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    // }


    private IEnumerator ThreeWaveAxeThrow()
    {
        int AttackRound = 1;
        isThrowing = true;

        while (AttackRound > 0)
        {

            GameObject Axe1 = Instantiate(axePrefab, firePoint.position, Canon1.rotation);
            GameObject Axe2 = Instantiate(axePrefab, firePoint.position, Canon2.rotation);
            GameObject Axe3 = Instantiate(axePrefab, firePoint.position, Canon3.rotation);

            Rigidbody2D axe1 = Axe1.GetComponent<Rigidbody2D>();
            Rigidbody2D axe2 = Axe2.GetComponent<Rigidbody2D>();
            Rigidbody2D axe3 = Axe3.GetComponent<Rigidbody2D>();
            Vector3 scale = transform.localScale;

            // axe1.AddForce(Axe1.transform.right * force, ForceMode2D.Impulse);
            // axe2.AddForce(Axe2.transform.right * force, ForceMode2D.Impulse);
            // axe3.AddForce(Axe3.transform.right * force, ForceMode2D.Impulse);
            float dis = Vector2.Distance(playerPos.position, MyselfCollider.transform.position);
            axe1.AddForce(new Vector2(scale.x * dis * 10, 1).normalized * force, ForceMode2D.Impulse);
            axe2.AddForce(new Vector2(scale.x * dis, 2).normalized * force, ForceMode2D.Impulse);
            axe3.AddForce(new Vector2(scale.x * dis / 10, 3).normalized * force, ForceMode2D.Impulse);
            // Debug.Log("dis: " + dis);

            // Debug.Log("Axe1: " + scale.x * dis + " " + axe1.velocity);
            // Debug.Log("Axe2: " + scale.x * dis + " " + axe2.velocity);
            // Debug.Log("Axe3: " + scale.x * dis + " " + axe3.velocity);

            yield return new WaitForSeconds(0.5f);
            AttackRound--;
        }
        isThrowing = false;
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
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere((Vector2)pivotPoint.position + shootRangePointOffset, rangedRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine((Vector2)pivotPoint.position + groundCheckOffset, (Vector2)pivotPoint.position + groundCheckOffset + new Vector2(0, -yGroundCheck));
        Gizmos.DrawLine((Vector2)pivotPoint.position + groundCheckOffset + new Vector2(xGroundCheck, 0), (Vector2)pivotPoint.position + groundCheckOffset + new Vector2(xGroundCheck, -yGroundCheck));
        Gizmos.DrawLine((Vector2)pivotPoint.position + groundCheckOffset + new Vector2(-xGroundCheck, 0), (Vector2)pivotPoint.position + groundCheckOffset + new Vector2(-xGroundCheck, -yGroundCheck));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)pivotPoint.position + R_WallCheckOffset, R_WallCheckSize);
        Gizmos.DrawWireCube((Vector2)pivotPoint.position + L_WallCheckOffset, L_WallCheckSize);
    }
}
