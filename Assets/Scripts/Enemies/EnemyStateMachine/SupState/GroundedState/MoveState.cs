using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : GroundState
{
    private bool is_change_direction = false;
    public MoveState(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyAttribute enemyAttribute, string anim_bool_name) : base(enemy, enemyStateMachine, enemyAttribute, anim_bool_name)
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

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (!is_change_direction)
            {
                if (enemy.EnemyPhysicCheck.CheckIfNeedToTurn(facing_direction > 0))
                {
                    is_change_direction = true;
                    enemy.Turn();
                }
            }

            if (facing_direction == enemy.transform.localScale.x)
                is_change_direction = false;
            // Debug.Log("時間: " + startTime + " 計數: " + Time.time);

            if (Time.time > startTime + enemyAttribute.MoveDuration)
            {
                enemyStateMachine.ChangeState(enemy.IdleState);
            }

        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.GroundMove(1, facing_direction * enemyAttribute.MaxFallSpeed, enemyAttribute.MoveMaxSpeed, enemyAttribute.MoveAccelAmount, enemyAttribute.MoveDeccelAmount);
    }

}
