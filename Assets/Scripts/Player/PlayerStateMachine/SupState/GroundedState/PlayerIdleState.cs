using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine, PlayerAttribute playerAttribute, string anim_bool_name) : base(player, playerStateMachine, playerAttribute, anim_bool_name)
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
                playerStateMachine.ChangeState(player.PlayerMoveState);
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
        player.GroundMove(1, 0, 0, playerAttribute.RunAccelAmount, playerAttribute.RunDeccelAmount);
    }
}