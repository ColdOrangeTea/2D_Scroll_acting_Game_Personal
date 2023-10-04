using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : EnemyState
{
    // protected float xInput;
    // protected float yInput;
    // protected bool jumpInput;
    // protected bool fireballInput;
    // protected bool airPushInput;
    // protected bool meleeInput;
    // protected bool dashInput;
    protected float facing_direction;
    protected bool isGrounded;
    protected bool isSawPlayer;

    public GroundState(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyAttribute enemyAttribute, string anim_bool_name) : base(enemy, enemyStateMachine, enemyAttribute, anim_bool_name)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = enemy.EnemyPhysicCheck.CheckIfGrounded();
        isSawPlayer = enemy.EnemyPhysicCheck.CheckIfSawPlayer();

    }

    public override void Enter()
    {
        base.Enter();
        facing_direction = enemy.transform.localScale.x;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isGrounded && enemy.EnemyPhysicCheck.CurrentVelocity.y < 0)
        {
            enemyStateMachine.ChangeState(enemy.InAirState);
        }
        if (isSawPlayer && enemy.MeleeAttackState.CheckIfCanAttack())
        {
            enemyStateMachine.ChangeState(enemy.MeleeAttackState);

            // if (Time.time >= startTime + enemyAttribute.MeleeCooldown)
            // {
            //     Debug.Log("攻擊!!!!!!!!!時間: " + Time.time + " 攻擊完畢的時候: " + startTime + "冷卻:" + enemyAttribute.MeleeCooldown);
            //     enemyStateMachine.ChangeState(enemy.MeleeAttackState);
            // }

        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
