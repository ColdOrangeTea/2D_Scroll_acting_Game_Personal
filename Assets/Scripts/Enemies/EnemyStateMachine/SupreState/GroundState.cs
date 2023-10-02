using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : EnemyState
{
    protected float xInput;
    protected float yInput;
    protected bool jumpInput;
    protected bool fireballInput;
    protected bool airPushInput;
    protected bool meleeInput;
    protected bool dashInput;
    protected bool isGrounded;

    public GroundState(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyAttribute enemyAttribute, string anim_bool_name) : base(enemy, enemyStateMachine, enemyAttribute, anim_bool_name)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = enemy.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();

        // playerMovement.FireballState.ResetCanFireball();
        // playerMovement.AirPushState.ResetCanAirPush();

        // enemy.PlayerDashState.ResetDashesLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        if (!isGrounded)
        {
            enemyStateMachine.ChangeState(enemy.InAirState);
        }
        else if (meleeInput)
        {
            if (yInput != -1)// on Groundand no Crouch
                enemyStateMachine.ChangeState(enemy.MeleeAttackState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
