using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MoveState : GroundedState
{
    public MoveState(Player player, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(player, stateMachine, unitAttribute, animBoolName)
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
            player.CheckDirectionToFace(xInput > 0);

        if (!isExitingState)
        {
            if (xInput == 0)
            {
                stateMachine.ChangeState(player.IdleState);
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

        player.GroundMove(1, xInput, unitAttribute.runMaxSpeed, unitAttribute.runAccelAmount, unitAttribute.runDeccelAmount);
    }
}
