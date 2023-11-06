using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Musketeer : NewEnemy
{
    // private float facing_direction;
    private bool isGrounded;
    private bool isChangeDirection;
    private float xScale;
    private bool isSawPlayer;
    [SerializeField] private float rangeAttackCoolDown;
    // [SerializeField] private float rangeAttackDuration;
    // [SerializeField] private float attackStartTime;
    [SerializeField] private float lastRangeTime;
    public float Force;
    public GameObject gun;
    public Transform shootpoint;
    public GameObject Bullet;
    [SerializeField] private Transform target;
    public override void Start()
    {
        base.Start();
        curState = State.idle.ToString();
        rangeAttackCoolDown = enemyAttribute.RangeAttackCooldown;
        // rangeAttackDuration = enemyAttribute.RangeAttackDuration;
        Force = enemyAttribute.RangeAttackFireForce;
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }
    public override void DoChecks()
    {
        base.DoChecks();
        // facing_direction = transform.localScale.x;
        isGrounded = NewEnemyPhysicsCheck.CheckIfGrounded();
    }
    public override void DoOverLapChecks()
    {
        isSawPlayer = NewEnemyPhysicsCheck.CheckIfSawPlayer();
        Debug.Log("玩家察覺: " + isSawPlayer);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isGrounded)
        {
            if (!isSawPlayer && curState != State.idle.ToString())
            {
                curState = State.idle.ToString();
            }
            if (isSawPlayer && curState != State.attack.ToString())
            {
                curState = State.attack.ToString();
            }

            if (!isChangeDirection)
            {
                float xTarget = target.position.x, xOwn = transform.position.x;
                if (xOwn - xTarget < 0)
                    xScale = 1;
                else
                    xScale = -1;
                if (xScale != transform.localScale.x)
                {
                    Turn();
                    isChangeDirection = true;
                    curState = State.idle.ToString();
                    Debug.Log("轉身了");
                }
            }

            if (isChangeDirection)
                isChangeDirection = false;
        }
        else
        {
            if (NewEnemyPhysicsCheck.RB.velocity.y < 0 && curState != State.fall.ToString())
                curState = State.fall.ToString();
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (curState == State.idle.ToString())
            IdleState();
        else if (curState == State.fall.ToString())
            FallState();
        else if (curState == State.attack.ToString())
            AttackState();
    }
    void IdleState()
    {
    }
    void FallState()
    {

    }
    void AttackState()
    {
        if (CheckIfCanRangeAttack())
        {
            lastRangeTime = Time.time;
            Shoot();
            Debug.Log("火槍攻擊開始 ");
        }

    }
    void DeadState()
    {

    }
    void Shoot()
    {
        Vector3 attackDir = target.transform.position - transform.position;
        GameObject BulletIns = Instantiate(Bullet, shootpoint.position, transform.rotation);
        BulletIns.GetComponent<Rigidbody2D>().velocity = attackDir * Force * 0.1f;
    }
    public bool CheckIfCanRangeAttack()
    {
        if (lastRangeTime == 0) // 時間 = 0 代表初次觸發攻擊
            return true;
        else
            return Time.time >= lastRangeTime + rangeAttackCoolDown;
    }

    public override void Turn()
    {
        //stores scale and flips the player along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x = xScale;
        transform.localScale = scale;
        Debug.Log("火槍轉身 ");
    }
}

// public class Musketeer : Enemy
// {
//     [SerializeField]
//     private EnemyAttribute musketeer_attribute;
//     // [SerializeField] private Core core;

//     protected override void Awake()
//     {
//         // core = GetComponent<Core>();
//         // Core = core;
//         enemyAttribute = musketeer_attribute;
//         Debug.Log("賦予資料" + enemyAttribute);
//         base.Awake();

//     }
// }
