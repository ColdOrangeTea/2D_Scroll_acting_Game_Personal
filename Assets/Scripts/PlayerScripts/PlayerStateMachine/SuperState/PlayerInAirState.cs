using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private float x_Input;
    private bool jump_cut_input;
    private bool jump_input;
    private bool double_jump_input;
    private bool melee_input;
    private bool dash_input;
    private bool is_grounded;
    private bool is_onwall;
    public bool IsJumping { get; private set; }
    public bool IsJumpCut { get; private set; }
    private int jump_count = 0;
    public PlayerInAirState(Player player, PlayerStateMachine playerStateMachine, PlayerAttribute playerAttribute, string anim_bool_name) : base(player, playerStateMachine, playerAttribute, anim_bool_name)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        player.PhysicsCheck.OnGroundCheck();
        is_grounded = player.PhysicsCheck.CheckIfGrounded();

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
        if (jump_input)
            double_jump_input = player.InputHandler.DoubleJumpInput;
        // fireballInput = playerMovement.inputHandler.FireballInput();
        // airPushInput = playerMovement.inputHandler.AirPushInput();
        melee_input = player.InputHandler.MeleeInput();
        dash_input = player.InputHandler.DashInput();

        Debug.Log("jump: " + IsJumping);
        if (x_Input != 0)
            player.PhysicsCheck.CheckDirectionToFace(x_Input > 0);
        if (is_grounded && player.PhysicsCheck.CurrentVelocity.y < 0.01f)
        {
            IsJumpCut = false;
            playerStateMachine.ChangeState(player.PlayerLandState);
        }
        if (jump_input && double_jump_input && jump_count == 0)
        {
            jump_count += 1;
            player.InputHandler.SetDoubleJumpInput(true);
            playerStateMachine.ChangeState(player.PlayerJumpState);
        }
        else if (melee_input && player.PlayerPunchState.CheckIfCanPunch())
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
        player.InAirMove(1, x_Input, playerAttribute.RunMaxSpeed, playerAttribute.RunAccelAmount * playerAttribute.AccelInAir,
        playerAttribute.RunDeccelAmount * playerAttribute.DeccelInAir, playerAttribute.JumpHangTimeThreshold, playerAttribute.JumpHangAccelerationMult,
        playerAttribute.JumpHangMaxSpeedMult, playerAttribute.DoConserveMomentum, IsJumping);
    }

    private void CheckJumping()
    {
        // if (IsJumping && player.PhysicsCheck.CurrentVelocity.y < 0)
        //     IsJumping = false;
        if (is_grounded)
        {
            IsJumping = false;
            jump_count = 0;
        }

    }
    private void CheckJumpCut()
    {
        if (jump_cut_input && CanJumpCut())
        {
            IsJumpCut = true;
        }
    }

    //public bool CanWallJumpCut() => player.WallJumpState.IsWallJumping && player.CurrentVelocity.y > 0;
    public bool CanJumpCut() => IsJumping && player.PhysicsCheck.CurrentVelocity.y > 0;

    public void SetJumping(bool setting) => IsJumping = setting;
    public void SetJumpCut(bool setting) => IsJumpCut = setting;
    public bool CanWallSlide() => !IsJumping && is_onwall && XInputAtWall() && player.PhysicsCheck.CurrentVelocity.y < 0;
    public bool XInputAtWall() => (player.PhysicsCheck.LastOnWallLeftTime > 0 && x_Input < 0) || (player.PhysicsCheck.LastOnWallRightTime > 0 && x_Input > 0);


}
