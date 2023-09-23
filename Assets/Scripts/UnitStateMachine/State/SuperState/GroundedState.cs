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

    public GroundedState(PlayerMovement playerMovement, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(playerMovement, stateMachine, unitAttribute, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        playerMovement.OnGroundCheck();
        isGrounded = playerMovement.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();

        // playerMovement.FireballState.ResetCanFireball();
        // playerMovement.AirPushState.ResetCanAirPush();
        playerMovement.DashState.ResetDashesLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = playerMovement.inputHandler.XInput;
        yInput = playerMovement.inputHandler.YInput;
        jumpInput = playerMovement.inputHandler.JumpInput();
        // fireballInput = playerMovement.inputHandler.FireballInput();
        // airPushInput = playerMovement.inputHandler.AirPushInput();
        meleeInput = playerMovement.inputHandler.MeleeInput();
        DashInput = playerMovement.inputHandler.DashInput();
        if (jumpInput && isGrounded)
        {
            stateMachine.ChangeState(playerMovement.JumpState);
        }
        else if (!isGrounded)
        {
            stateMachine.ChangeState(playerMovement.InAirState);
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
                stateMachine.ChangeState(playerMovement.AttackState);
        }
        else if (DashInput && yInput != -1 && playerMovement.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(playerMovement.DashState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

