using UnityEngine;
using UnityHFSM;

public class MusketeerHFSMStateManager : MonoBehaviour
{
    #region COMPONENTS
    [Header("Component")]
    private Rigidbody2D rb;
    public Collider2D MyselfCollider;
    private StateMachine fsm;
    private Animator animator;
    public GameObject Bullet;
    public GameObject gun;
    #endregion

    [Header("Checksbox")]
    public Transform pivotPoint;
    public Vector2 fireDir;
    [Space(10)]

    [Header("Status")]
    public bool isFacingRight = true;
    public bool inShootRange;
    public bool canShoot = true;
    bool isShoot = false;
    [Space(10)]

    [Header("Adjustment")]
    public Transform playerPos;//chage to private
    public float ShootCooldown = 3f;
    public float Force;

    // [SerializeField] private float lastShootTime;

    #region ATTACK
    public Vector2 rangedPointoffset;
    public float rangedRadius = 8f;
    public Vector2 shootPointoffset;
    #endregion

    #region  LAYER
    public LayerMask AttackLayer;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MyselfCollider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        fsm = new StateMachine();
        fsm.AddState("idle", onEnter: state => animator.SetBool("idle", true),
            onLogic: state =>
            {
                FacingPlayer();
                DetectPlayer();
            }, onExit: state => animator.SetBool("idle", false));

        fsm.AddState("rangedAttack", onEnter: state => { SetUnAbleToShoot(); animator.SetBool("rangedAttack", true); },
        onLogic: state =>
        {
            FacingPlayer();
            DetectPlayer();
            if (!isShoot)
                Shoot();
            // Debug.Log("可開槍動畫狀態: " + AnimatorIsPlaying("rangedAttack"));
        }, onExit: state => { }, canExit: state => !AnimatorIsPlaying("rangedAttack"), needsExitTime: true);

        fsm.AddTransition("rangedAttack", "idle", t => !canShoot || !inShootRange);
        fsm.AddTransition("idle", "rangedAttack", t => canShoot && inShootRange);
        // fsm.AddTransitionFromAny("rangedAttack", t => inShootRange && canShoot);
        fsm.SetStartState("idle");
        fsm.Init();
    }
    void Update()
    {
        fsm.OnLogic();
        // inShootRange = Physics2D.OverlapCircle((Vector2)pivotPoint.position + rangedPointoffset, rangedRadius, AttackLayer).CompareTag("Player");
        // Debug.Log("當前State: " + fsm.GetActiveHierarchyPath() + " 在射擊範圍內?: " + inShootRange + " canShoot: " + canShoot);
        fireDir = (Vector2)playerPos.position - (Vector2)transform.position;
    }
    void DetectPlayer()
    {
        if (Vector2.Distance(this.gameObject.transform.position, playerPos.position) < rangedRadius)
        {
            inShootRange = true;
        }
        else
        {
            inShootRange = false;
        }
    }
    public void FacingPlayer()
    {
        if (playerPos.position.x > transform.position.x == isFacingRight)
            Turn();
        //if (playerPos.position.x < transform.position.x != isFacingRight)
        //    Turn();
    }
    public void Turn()
    {
        //stores scale and flips the enemy along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }
    void Shoot()
    {
        isShoot = true;
        // Debug.Log("射擊");
        Vector3 attackDir = (playerPos.transform.position - transform.position).normalized;
        gun.transform.right = -fireDir;
        GameObject BulletIns = Instantiate(Bullet, (Vector2)pivotPoint.position + shootPointoffset, gun.transform.rotation);
        BulletIns.GetComponent<Rigidbody2D>().velocity = attackDir * Force * 0.1f;
    }
    private void SetUnAbleToShoot()
    {
        canShoot = false;
        isShoot = false;
        CancelInvoke("SetAbletoShoot");
        Invoke("SetAbletoShoot", ShootCooldown);
    }
    private void SetAbletoShoot()
    {
        canShoot = true;
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)pivotPoint.position + rangedPointoffset, rangedRadius);
        Gizmos.DrawWireSphere((Vector2)pivotPoint.position + shootPointoffset, 0.2f);

    }
}
