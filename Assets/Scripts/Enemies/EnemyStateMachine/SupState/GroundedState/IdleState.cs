using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : GroundState
{
    public IdleState(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyAttribute enemyAttribute, string anim_bool_name) : base(enemy, enemyStateMachine, enemyAttribute, anim_bool_name)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (xInput != 0)
            enemy.CheckDirectionToFace(xInput > 0);

        if (!isExitingState)
        {
            if (xInput != 0)
            {
                enemyStateMachine.ChangeState(enemy.MoveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.GroundMove(1, 0, 0, enemyAttribute.MoveAccelAmount, enemyAttribute.MoveDeccelAmount);
    }
}
