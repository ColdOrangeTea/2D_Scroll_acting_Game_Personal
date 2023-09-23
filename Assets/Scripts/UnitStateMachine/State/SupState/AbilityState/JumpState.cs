using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : AbilityState
{
    public JumpState(PlayerMovement playerMovement, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(playerMovement, stateMachine, unitAttribute, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        playerMovement.inputHandler.UseJumpInput();
        playerMovement.Jump();

        isAbilityDone = true;

        playerMovement.InAirState.SetJumping(true);
        playerMovement.InAirState.SetJumpCut(false);
    }
}
