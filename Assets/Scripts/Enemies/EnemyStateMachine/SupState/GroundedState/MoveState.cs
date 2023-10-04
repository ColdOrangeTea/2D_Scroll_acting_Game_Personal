using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : GroundState
{

    public float x_localscale { get; private set; }
    private bool is_touchingwall = false;

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
        x_localscale = enemy.transform.localScale.x;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (!is_change_direction)
            {
                if (enemy.EnemyPhysicCheck.CheckIfNeedToTurn(x_localscale > 0))
                {
                    is_change_direction = true;
                    enemy.Turn();
                }
            }

            if (x_localscale == enemy.transform.localScale.x)
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
        enemy.GroundMove(1, x_localscale * enemyAttribute.MaxFallSpeed, enemyAttribute.MoveMaxSpeed, enemyAttribute.MoveAccelAmount, enemyAttribute.MoveDeccelAmount);
    }

}
