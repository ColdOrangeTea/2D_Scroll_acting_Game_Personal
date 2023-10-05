using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandState : GroundState
{
    public LandState(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyAttribute enemyAttribute, string anim_bool_name) : base(enemy, enemyStateMachine, enemyAttribute, anim_bool_name)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (facing_direction != 0)
            enemy.EnemyPhysicCheck.CheckIfNeedToTurn(facing_direction > 0);

        if (!isExitingState)
        {
            if (enemyAttribute.ThisEnemyIsCanMove)
            {
                if (enemy.EnemyPhysicCheck.CurrentVelocity != Vector2.zero)
                {
                    enemyStateMachine.ChangeState(enemy.MoveState);
                }
                else
                    enemyStateMachine.ChangeState(enemy.IdleState);
            }
            else
            {
                enemyStateMachine.ChangeState(enemy.IdleState);
            }

        }
    }
    public override void PhysicsUpdate()
    {
        base.LogicUpdate();
    }
}
