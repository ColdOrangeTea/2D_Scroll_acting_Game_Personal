using UnityEngine;
using UnityEngine.UI;
using UnityHFSM;

public class MechanicHoundHFSMStateManager : MonoBehaviour
{
    #region COMPONENTS
    [Header("Component")]
    private Rigidbody2D rb;
    public Collider2D MyselfCollider;
    private StateMachine fsm;
    private Animator animator;
    private Text stateDisplayText;
    #endregion

    [Header("Checksbox")]
    public Transform pivotPoint;
    [Space(10)]

    [Header("Status")]
    public bool isFacingRight = true;
    [Space(10)]

    [Header("Adjustment")]
    #region IDLE
    public Vector2 playerCheckOffset;
    public Vector2 playerCheckSize;
    #endregion

    #region CHASE
    public float chaseSpeed = 2f;
    public Transform playerPos;//chage to private
    public Vector2 playerposition;
    #endregion

    #region  LAYER
    public LayerMask GroundLayer;
    public LayerMask AttackableUnitLayer;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MyselfCollider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        stateDisplayText = GetComponentInChildren<Text>();
        fsm = new StateMachine();
        fsm.AddState(HFSMState.idle.ToString(), onEnter: state => animator.Play(HFSMState.idle.ToString()));
        fsm.AddState(HFSMState.chase.ToString(), onEnter: state => animator.Play(HFSMState.chase.ToString()),
        onLogic: state =>
        {
            // Debug.Log("追逐中");
            FacingPlayer();
            rb.velocity = new Vector2(chaseSpeed * (isFacingRight ? -1 : 1), rb.velocity.y);
        });
        fsm.AddTransition(HFSMState.idle.ToString(), HFSMState.chase.ToString(), t => SawPlayer());
        // fsm.AddTransition(HFSMState.chase.ToString(), HFSMState.idle.ToString(), t => !SawPlayer());
        fsm.SetStartState(HFSMState.idle.ToString());
        fsm.Init();
    }
    void Update()
    {
        playerposition = playerPos.position;
        fsm.OnLogic();
        stateDisplayText.text = fsm.GetActiveHierarchyPath();
        Debug.Log("看到: " + SawPlayer());
    }
    public bool SawPlayer()
    {
        if (Physics2D.OverlapBox((Vector2)pivotPoint.position + playerCheckOffset, playerCheckSize, 0, AttackableUnitLayer).CompareTag("Player"))
        {
            return true;
        }
        else
            return false;
    }
    public void FacingPlayer()
    {
        if (playerPos.position.x < transform.position.x != isFacingRight)
            Turn();
        // if (playerPos.position.x < transform.position.x != isFacingRight)
        //     Turn();
    }
    public void Turn()
    {
        //stores scale and flips the enemy along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        // Gizmos.DrawWireSphere((Vector2)pivotPoint.position + combatPointOffset, combatRadius);
        Gizmos.color = Color.red;
        //  Gizmos.DrawWireSphere((Vector2)pivotPoint.position + rangedPointoffset, rangedRadius);
        Gizmos.color = Color.cyan;
        //  Gizmos.DrawWireCube((Vector2)pivotPoint.position + platformCheckPointoffset, platformChecksize);
        Gizmos.DrawWireCube((Vector2)pivotPoint.position + playerCheckOffset, playerCheckSize);

    }
}
