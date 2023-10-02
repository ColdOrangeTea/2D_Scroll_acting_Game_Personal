using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityState : EnemyState
{
    protected bool isAbilityDone;
    protected bool isGrounded;
    protected float xInput;

    public AbilityState(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyAttribute enemyAttribute, string anim_bool_name) : base(enemy, enemyStateMachine, enemyAttribute, anim_bool_name)
    {
    }
    public override void DoChecks() //fixedupdate
    {
        base.DoChecks();

        isGrounded = enemy.CheckIfGrounded();
        //Debug.Log(isGrounded);
        //Debug.Log(Time.time);
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // xInput = enemy.InputHandler.XInput;

        if (isAbilityDone)
        {
            // if (isGrounded && playerMovement.inputHandler.YInput == -1)
            // {
            //     stateMachine.ChangeState(playerMovement.CrouchIdleState);
            // }
            if (isGrounded && enemy.CurrentVelocity.y < 0.01f)
            {
                enemyStateMachine.ChangeState(enemy.IdleState);
            }
            else
            {
                enemyStateMachine.ChangeState(enemy.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }
}
