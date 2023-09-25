using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : AbilityState
{
    public JumpState(Player player, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(player, stateMachine, unitAttribute, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.inputHandler.UseJumpInput();
        player.Jump();

        isAbilityDone = true;

        player.InAirState.SetJumping(true);
        player.InAirState.SetJumpCut(false);
    }
}
