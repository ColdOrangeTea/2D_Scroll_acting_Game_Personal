using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : State
{
    protected float xInput;
    protected float yInput;
    protected bool jumpInput;
    protected bool fireballInput;
    protected bool airPushInput;
    protected bool meleeInput;
    protected bool DashInput;
    protected bool isGrounded;

    public GroundedState(Player player, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(player, stateMachine, unitAttribute, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        player.OnGroundCheck();
        isGrounded = player.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();

        // playerMovement.FireballState.ResetCanFireball();
        // playerMovement.AirPushState.ResetCanAirPush();
        player.DashState.ResetDashesLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.inputHandler.XInput;
        yInput = player.inputHandler.YInput;
        jumpInput = player.inputHandler.JumpInput();
        // fireballInput = playerMovement.inputHandler.FireballInput();
        // airPushInput = playerMovement.inputHandler.AirPushInput();
        meleeInput = player.inputHandler.MeleeInput();
        DashInput = player.inputHandler.DashInput();
        if (jumpInput && isGrounded)
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!isGrounded)
        {
            stateMachine.ChangeState(player.InAirState);
        }
        // else if (fireballInput && playerMovement.FireballState.CheckIfCanFireball())
        // {
        //     stateMachine.ChangeState(playerMovement.FireballState);
        // }
        // else if (airPushInput && playerMovement.AirPushState.CheckIfCanAirPush())
        // {
        //     stateMachine.ChangeState(playerMovement.AirPushState);
        // }
        else if (meleeInput)
        {
            if (yInput != -1)// on Groundand no Crouch
                stateMachine.ChangeState(player.AttackState);
        }
        else if (DashInput && yInput != -1 && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

