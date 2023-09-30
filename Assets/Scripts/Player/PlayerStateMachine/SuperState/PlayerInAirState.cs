using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private float x_Input;
    private bool jump_cut_input;
    private bool jump_input;
    private bool fireball_input;
    private bool airpush_input;
    private bool melee_input;
    private bool dash_input;
    private bool is_grounded;
    private bool is_onwall;
    public bool IsJumping { get; private set; }
    public bool IsJumpCut { get; private set; }
    private bool can_grab;
    public PlayerInAirState(Player player, PlayerStateMachine playerStateMachine, PlayerAttribute playerAttribute, string anim_bool_name) : base(player, playerStateMachine, playerAttribute, anim_bool_name)
    {
    }


    public override void DoChecks()
    {
        base.DoChecks();

        player.OnGroundCheck();
        is_grounded = player.CheckIfGrounded();

        // playerMovement.OnWallCheck();
        // isOnWall = playerMovement.CheckIfOnWall();

        //Debug.Log(isGrounded);
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
        //Local Assign vvv remember
        x_Input = player.InputHandler.XInput;
        jump_cut_input = player.InputHandler.JumpCutInput;
        jump_input = player.InputHandler.JumpInput();
        // fireballInput = playerMovement.inputHandler.FireballInput();
        // airPushInput = playerMovement.inputHandler.AirPushInput();
        melee_input = player.InputHandler.MeleeInput();
        dash_input = player.InputHandler.DashInput();

        // canGrab = playerMovement.CanGrab();

        if (x_Input != 0)
            player.CheckDirectionToFace(x_Input > 0);

        if (is_grounded && player.CurrentVelocity.y < 0.01f)
        {
            IsJumpCut = false;
            playerStateMachine.ChangeState(player.PlayerLandState);
        }
        // else if (isOnWall && jumpInput)
        // {
        //     stateMachine.ChangeState(playerMovement.WallJumpState);
        // }
        // else if (CanWallGrab())
        // {
        //     stateMachine.ChangeState(playerMovement.WallGrabState);
        // }
        // else if (CanWallSlide())
        // {
        //     stateMachine.ChangeState(playerMovement.WallSlideState);
        // }
        // else if (fireballInput && playerMovement.FireballState.CheckIfCanFireball())
        // {
        //     stateMachine.ChangeState(playerMovement.FireballState);
        // }
        // else if (airPushInput && playerMovement.AirPushState.CheckIfCanAirPush())
        // {
        //     stateMachine.ChangeState(playerMovement.AirPushState);
        // }
        else if (melee_input && player.PlayerPunchState.CheckIfCanSlash())
        {
            playerStateMachine.ChangeState(player.PlayerPunchState);
        }
        else if (dash_input && player.PlayerDashState.CheckIfCanDash())
        {
            playerStateMachine.ChangeState(player.PlayerDashState);
        }



        CheckJumping();
        CheckJumpCut();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.InAirMove(1, x_Input, playerAttribute.runMaxSpeed, playerAttribute.runAccelAmount * playerAttribute.accelInAir,
        playerAttribute.runDeccelAmount * playerAttribute.deccelInAir, playerAttribute.jumpHangTimeThreshold, playerAttribute.jumpHangAccelerationMult,
        playerAttribute.jumpHangMaxSpeedMult, playerAttribute.doConserveMomentum, IsJumping);
    }

    private void CheckJumping()
    {
        if (IsJumping && player.CurrentVelocity.y < 0)
            IsJumping = false;
    }
    private void CheckJumpCut()
    {
        if (jump_cut_input && CanJumpCut())
        {
            IsJumpCut = true;
        }
    }

    //public bool CanWallJumpCut() => player.WallJumpState.IsWallJumping && player.CurrentVelocity.y > 0;
    public bool CanJumpCut() => IsJumping && player.CurrentVelocity.y > 0;

    public void SetJumping(bool setting) => IsJumping = setting;
    public void SetJumpCut(bool setting) => IsJumpCut = setting;
    public bool CanWallSlide() => !IsJumping && is_onwall && XInputAtWall() && player.CurrentVelocity.y < 0;
    public bool CanWallGrab() => !IsJumping && is_onwall && XInputAtWall() && can_grab;
    public bool XInputAtWall() => (player.LastOnWallLeftTime > 0 && x_Input < 0) || (player.LastOnWallRightTime > 0 && x_Input > 0);


}
