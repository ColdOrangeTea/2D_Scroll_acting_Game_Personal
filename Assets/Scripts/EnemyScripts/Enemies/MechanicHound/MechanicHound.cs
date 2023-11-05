using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class MechanicHound : NewEnemy
{
    public float distance;
    // private float facing_direction;
    private bool isGrounded;
    private bool isChangeDirection;

    [SerializeField] private Transform target;
    public override void Start()
    {
        base.Start();
        curState = State.chase.ToString();
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
    public override void LogicUpdate()
    {
        if (isGrounded && curState != State.chase.ToString())
        {
            curState = State.chase.ToString();
        }
        else if (!isGrounded && NewEnemyPhysicsCheck.RB.velocity.y < 0)
        {
            curState = State.fall.ToString();
        }

        if (curState == State.chase.ToString())
        {
            if (!isChangeDirection)
            {
                if (NewEnemyPhysicsCheck.CheckIfNeedToTurn())
                {
                    Turn();
                    isChangeDirection = true;
                    curState = State.chase.ToString();
                }
            }
            if (isChangeDirection)
                isChangeDirection = false;
        }

    }
    public override void PhysicsUpdate()
    {
        distance = Vector3.Distance(target.position, transform.position);
        base.PhysicsUpdate();
        if (curState == State.chase.ToString())
            ChaseState();
        else if (curState == State.fall.ToString())
            FallState();
    }
    void ChaseState()
    {
        float chaseDistance = Mathf.Abs(Vector3.Distance(transform.position, target.position));
        Vector3 dir = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (chaseDistance > 1.5)
        {
            GroundMove(1, angle * enemyAttribute.MaxFallSpeed, enemyAttribute.MoveMaxSpeed, enemyAttribute.MoveAccelAmount, enemyAttribute.MoveDeccelAmount);
            // Debug.Log("距離: " + chaseDistance);
        }

    }
    void FallState()
    {

    }
    void DeadState()
    {

    }


}
