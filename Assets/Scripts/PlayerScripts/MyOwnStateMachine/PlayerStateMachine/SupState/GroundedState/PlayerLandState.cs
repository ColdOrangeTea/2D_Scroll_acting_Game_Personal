using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, PlayerStateMachine playerStateMachine, PlayerAttribute playerAttribute, string anim_bool_name) : base(player, playerStateMachine, playerAttribute, anim_bool_name)
    {
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (xInput != 0)
            player.PhysicsCheck.CheckDirectionToFace(xInput > 0);

        if (!isExitingState)
        {
            if (xInput != 0)
            {
                playerStateMachine.ChangeState(player.PlayerMoveState);
            }
            else
                playerStateMachine.ChangeState(player.PlayerIdleState);
        }
    }
}

