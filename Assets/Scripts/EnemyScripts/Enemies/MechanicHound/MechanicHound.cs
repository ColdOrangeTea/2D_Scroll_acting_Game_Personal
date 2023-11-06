using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class MechanicHound : NewEnemy
{
    public float chaseDistance;
    // private float facing_direction;
    private bool isGrounded;
    private bool isChangeDirection;
    private bool isSawPlayer;
    [SerializeField] private Transform target;
    public override void Start()
    {
        base.Start();
        curState = State.idle.ToString();
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
            if (isSawPlayer && curState != State.chase.ToString())
            {
                curState = State.chase.ToString();
            }

            if (!isChangeDirection)
            {
                if (NewEnemyPhysicsCheck.CheckIfNeedToTurn())
                {
                    Turn();
                    isChangeDirection = true;
                    // curState = State.idle.ToString();
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
        else if (curState == State.chase.ToString())
            ChaseState();
    }
    void IdleState()
    {

    }
    void FallState()
    {

    }
    void ChaseState()
    {
        // chaseDistance = Mathf.Abs(target.position.x - transform.position.x) + Mathf.Abs(target.position.y - transform.position.y);
        Vector3 dir = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //    if (chaseDistance > 1)
        //     {
        GroundMove(1, angle * enemyAttribute.MaxFallSpeed, enemyAttribute.MoveMaxSpeed, enemyAttribute.MoveAccelAmount, enemyAttribute.MoveDeccelAmount);
        //    }

    }
    void DeadState()
    {

    }


}
