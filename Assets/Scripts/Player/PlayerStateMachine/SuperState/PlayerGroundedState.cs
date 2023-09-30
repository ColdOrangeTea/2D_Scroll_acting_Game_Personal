using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected float xInput;
    protected float yInput;
    protected bool jumpInput;
    protected bool fireballInput;
    protected bool airPushInput;
    protected bool meleeInput;
    protected bool dashInput;
    protected bool isGrounded;

    public PlayerGroundedState(Player player, PlayerStateMachine playerStateMachine, UnitAttribute unitAttribute, string anim_bool_name) : base(player, playerStateMachine, unitAttribute, anim_bool_name)
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
        player.PlayerDashState.ResetDashesLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.XInput;
        yInput = player.InputHandler.YInput;
        jumpInput = player.InputHandler.JumpInput();
        // fireballInput = playerMovement.inputHandler.FireballInput();
        // airPushInput = playerMovement.inputHandler.AirPushInput();
        meleeInput = player.InputHandler.MeleeInput();
        dashInput = player.InputHandler.DashInput();
        if (jumpInput && isGrounded)
        {
            playerStateMachine.ChangeState(player.PlayerJumpState);
        }
        else if (!isGrounded)
        {
            playerStateMachine.ChangeState(player.PlayerInAirState);
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
                playerStateMachine.ChangeState(player.PlayerPunchState);
        }
        else if (dashInput && yInput != -1 && player.PlayerDashState.CheckIfCanDash())
        {
            playerStateMachine.ChangeState(player.PlayerDashState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

