using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMachine : NewEnemy
{
    private float facing_direction;
    private bool isGrounded;
    private bool isChangeDirection;
    public override void Start()
    {
        base.Start();
        curState = State.walk.ToString();
    }
    public override void DoChecks()
    {
        base.DoChecks();
        facing_direction = transform.localScale.x;
        isGrounded = NewEnemyPhysicsCheck.CheckIfGrounded();
    }
    public override void LogicUpdate()
    {
        if (isGrounded && curState != State.walk.ToString())
        {
            curState = State.walk.ToString();
        }
        else if (!isGrounded && NewEnemyPhysicsCheck.RB.velocity.y < 0)
        {
            curState = State.fall.ToString();
        }
        if (!isChangeDirection)
        {
            if (NewEnemyPhysicsCheck.CheckIfNeedToTurn())
            {
                Turn();
                isChangeDirection = true;
                curState = State.walk.ToString();
            }
        }
        if (isChangeDirection)
            isChangeDirection = false;
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (curState == State.walk.ToString())
            WalkState();
        else if (curState == State.fall.ToString())
            FallState();
    }
    void WalkState()
    {
        GroundMove(1, facing_direction * enemyAttribute.MaxFallSpeed, enemyAttribute.MoveMaxSpeed, enemyAttribute.MoveAccelAmount, enemyAttribute.MoveDeccelAmount);
    }
    void FallState()
    {

    }
    void DeadState()
    {

    }

}

// public class MeleeMachine : Enemy
// {
//     [SerializeField]
//     private EnemyAttribute meleeMachine_attribute;
//     // [SerializeField] private Core core;

//     protected override void Awake()
//     {
//         // core = GetComponent<Core>();
//         // Core = core;
//         enemyAttribute = meleeMachine_attribute;
//         Debug.Log("賦予資料" + enemyAttribute);
//         base.Awake();

//     }
// }
