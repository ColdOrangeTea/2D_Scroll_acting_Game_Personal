using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine, PlayerAttribute playerAttribute, string anim_bool_name) : base(player, playerStateMachine, playerAttribute, anim_bool_name)
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
