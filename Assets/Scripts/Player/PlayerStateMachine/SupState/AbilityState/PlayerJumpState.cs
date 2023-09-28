using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine, UnitAttribute unitAttribute, string anim_bool_name) : base(player, playerStateMachine, unitAttribute, anim_bool_name)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseJumpInput();
        player.Jump();

        isAbilityDone = true;

        player.PlayerInAirState.SetJumping(true);
        player.PlayerInAirState.SetJumpCut(false);
    }
}
