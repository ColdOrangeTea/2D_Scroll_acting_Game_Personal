using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandState : GroundedState
{
    public LandState(PlayerMovement playerMovement, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(playerMovement, stateMachine, unitAttribute, animBoolName)
    {
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (xInput != 0)
            playerMovement.CheckDirectionToFace(xInput > 0);

        if (!isExitingState)
        {
            if (xInput != 0)
            {
                stateMachine.ChangeState(playerMovement.MoveState);
            }
            else
                stateMachine.ChangeState(playerMovement.IdleState);
        }
    }
}

