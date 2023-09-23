using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingWallState : State
{
    protected bool isGrounded;
    protected bool isOnWall;
    protected float xInput;
    protected bool jumpInput;
    public bool IsOnWall { get; private set; }

    public TouchingWallState(PlayerMovement playerMovement, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(playerMovement, stateMachine, unitAttribute, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        playerMovement.OnGroundCheck();
        isGrounded = playerMovement.CheckIfGrounded();

        // playerMovement.OnWallCheck();
        // isOnWall = playerMovement.CheckIfOnWall();
    }

    public override void Enter()
    {
        base.Enter();

        IsOnWall = true;
    }

    public override void Exit()
    {
        base.Exit();

        IsOnWall = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // xInput = playerMovement.inputHandler.XInput;
        // jumpInput = playerMovement.inputHandler.JumpInput();

        // if (jumpInput)
        // {
        //     stateMachine.ChangeState(playerMovement.WallJumpState);
        // }
        // else if (isGrounded)
        // {
        //     stateMachine.ChangeState(playerMovement.IdleState);
        // }
        // else if (!OnWall())
        // {
        //     stateMachine.ChangeState(playerMovement.InAirState);
        // }


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public bool OnWall() => isOnWall && XInputAtWall();
    public bool XInputAtWall() => (playerMovement.LastOnWallLeftTime > 0 && xInput < 0) || (playerMovement.LastOnWallRightTime > 0 && xInput > 0);
}
