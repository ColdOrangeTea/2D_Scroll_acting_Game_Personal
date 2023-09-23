using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MoveState : GroundedState
{
    public MoveState(PlayerMovement playerMovement, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(playerMovement, stateMachine, unitAttribute, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
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

        if (xInput != 0)
            playerMovement.CheckDirectionToFace(xInput > 0);

        if (!isExitingState)
        {
            if (xInput == 0)
            {
                stateMachine.ChangeState(playerMovement.IdleState);
            }
            // else if (yInput == -1 && Mathf.Abs(playerMovement.CurrentVelocity.x) >= unitAttribute.runMaxSpeed * 0.9f)
            // {
            //     stateMachine.ChangeState(playerMovement.CrouchSlideState);
            // }
            // else if (yInput == -1 && Mathf.Abs(playerMovement.CurrentVelocity.x) < unitAttribute.runMaxSpeed * 0.9f)
            // {
            //     stateMachine.ChangeState(playerMovement.CrouchIdleState);
            // }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        playerMovement.GroundMove(1, xInput, unitAttribute.runMaxSpeed, unitAttribute.runAccelAmount, unitAttribute.runDeccelAmount);
    }
}
