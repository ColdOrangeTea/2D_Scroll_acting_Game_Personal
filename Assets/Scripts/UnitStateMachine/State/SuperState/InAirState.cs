using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirState : State
{
    private float xInput;
    private bool jumpCutInput;
    private bool jumpInput;
    private bool fireballInput;
    private bool airPushInput;
    private bool meleeInput;
    private bool dashInput;
    private bool isGrounded;
    private bool isOnWall;
    public bool IsJumping { get; private set; }
    public bool IsJumpCut { get; private set; }
    private bool canGrab;
    public InAirState(Player player, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(player, stateMachine, unitAttribute, animBoolName)
    {
    }


    public override void DoChecks()
    {
        base.DoChecks();

        player.OnGroundCheck();
        isGrounded = player.CheckIfGrounded();

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
        xInput = player.inputHandler.XInput;
        jumpCutInput = player.inputHandler.JumpCutInput;
        jumpInput = player.inputHandler.JumpInput();
        // fireballInput = playerMovement.inputHandler.FireballInput();
        // airPushInput = playerMovement.inputHandler.AirPushInput();
        meleeInput = player.inputHandler.MeleeInput();
        dashInput = player.inputHandler.DashInput();

        // canGrab = playerMovement.CanGrab();

        if (xInput != 0)
            player.CheckDirectionToFace(xInput > 0);

        if (isGrounded && player.CurrentVelocity.y < 0.01f)
        {
            IsJumpCut = false;
            stateMachine.ChangeState(player.LandState);
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
        else if (meleeInput && player.AttackState.CheckIfCanSlash())
        {
            stateMachine.ChangeState(player.AttackState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }



        CheckJumping();
        CheckJumpCut();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.InAirMove(1, xInput, unitAttribute.runMaxSpeed, unitAttribute.runAccelAmount * unitAttribute.accelInAir,
        unitAttribute.runDeccelAmount * unitAttribute.deccelInAir, unitAttribute.jumpHangTimeThreshold, unitAttribute.jumpHangAccelerationMult,
        unitAttribute.jumpHangMaxSpeedMult, unitAttribute.doConserveMomentum, IsJumping);
    }

    private void CheckJumping()
    {
        if (IsJumping && player.CurrentVelocity.y < 0)
            IsJumping = false;
    }
    private void CheckJumpCut()
    {
        if (jumpCutInput && CanJumpCut())
        {
            IsJumpCut = true;
        }
    }

    //public bool CanWallJumpCut() => player.WallJumpState.IsWallJumping && player.CurrentVelocity.y > 0;
    public bool CanJumpCut() => IsJumping && player.CurrentVelocity.y > 0;

    public void SetJumping(bool setting) => IsJumping = setting;
    public void SetJumpCut(bool setting) => IsJumpCut = setting;
    public bool CanWallSlide() => !IsJumping && isOnWall && XInputAtWall() && player.CurrentVelocity.y < 0;
    public bool CanWallGrab() => !IsJumping && isOnWall && XInputAtWall() && canGrab;
    public bool XInputAtWall() => (player.LastOnWallLeftTime > 0 && xInput < 0) || (player.LastOnWallRightTime > 0 && xInput > 0);


}
