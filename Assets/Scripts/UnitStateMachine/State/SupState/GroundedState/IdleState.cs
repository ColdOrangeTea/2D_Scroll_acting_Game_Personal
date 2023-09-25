using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class IdleState : GroundedState
{
    public IdleState(Player player, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(player, stateMachine, unitAttribute, animBoolName)
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
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            // else if (yInput == -1)
            // {
            //     stateMachine.ChangeState(playerMovement.CrouchIdleState);
            // }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.GroundMove(1, 0, 0, unitAttribute.runAccelAmount, unitAttribute.runDeccelAmount);
    }
}