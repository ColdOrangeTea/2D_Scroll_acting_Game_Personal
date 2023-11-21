using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;
using UnityHFSM;

public class MachineBearHFSMStateManager : MonoBehaviour
{
    #region COMPONENTS
    private Rigidbody2D rb;
    private StateMachine fsm;
    private Animator animator;
    private Text stateDisplayText;
    #endregion

    public Transform playerPos;//chage to private
    [Header("Adjustment")]
    public float chaseSpeed = 10f;

    public Vector2 jumpDir = new Vector2(0.1f, 1f);
    public float jumpForce = 5f;//can use newrton's law to adjust
    public float heightGap = 5f;

    [Header("Checksbox")]
    public Transform pivotPoint;
    public Vector2 platformCheckPointoffset;
    public Vector2 platformChecksize = new Vector2(0.5f, 1);

    [Header("Status")]
    public bool isFacingRight = true;
    public bool onHeadHavePlatform;

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
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        stateDisplayText = GetComponentInChildren<Text>();
        fsm = new StateMachine();
        fsm.AddState("Chase", onEnter: state => animator.Play("Walk"),
        onLogic: state =>
        {
            facingPlayer();
            rb.velocity = new Vector2(chaseSpeed * (isFacingRight ? -1 : 1), rb.velocity.y);
        });
        fsm.AddState("Jump", onEnter: state => { rb.AddForce(jumpDir * jumpForce, ForceMode2D.Impulse); animator.Play("LongJump"); }, onExit: state => rb.velocity = new Vector2(0, 0), canExit: state => !AnimatorIsPlaying("LongJump"), needsExitTime: true);
        fsm.AddTransition("Chase", "Jump", t => playerPos.position.y - transform.position.y > heightGap && onHeadHavePlatform);

        fsm.AddTransitionFromAny("Chase");
        fsm.SetStartState("Chase");
        fsm.Init();
    }

    // Update is called once per frame
    void Update()
    {
        fsm.OnLogic();
        stateDisplayText.text = fsm.GetActiveHierarchyPath();
        onHeadHavePlatform = Physics2D.OverlapBox((Vector2)pivotPoint.position + platformCheckPointoffset, platformChecksize, 0, groundLayer);
    }
    public void facingPlayer()
    {
        if (playerPos.position.x < transform.position.x != isFacingRight)
            Turn();
    }
    public void Turn()
    {
        //stores scale and flips the enemy along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }

    private void SetIgnoreCollision()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerPos.GetComponent<Collider2D>());
    }

    bool AnyAnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    bool AnimatorIsPlaying(string stateName)
    {
        return AnyAnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube((Vector2)pivotPoint.position + platformCheckPointoffset, platformChecksize);

    }
}
