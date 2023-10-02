using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;
    protected bool isGrounded;
    protected float xInput;
    public PlayerAbilityState(Player player, PlayerStateMachine playerStateMachine, PlayerAttribute playerAttribute, string anim_bool_name) : base(player, playerStateMachine, playerAttribute, anim_bool_name)
    {
    }

    public override void DoChecks() //fixedupdate
    {
        base.DoChecks();

        player.OnGroundCheck();
        isGrounded = player.CheckIfGrounded();
        //Debug.Log(isGrounded);
        // Debug.Log(Time.time);
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.XInput;

        if (isAbilityDone)
        {
            // if (isGrounded && playerMovement.inputHandler.YInput == -1)
            // {
            //     stateMachine.ChangeState(playerMovement.CrouchIdleState);
            // }

            if (isGrounded && player.CurrentVelocity.y < 0.01f)
            {
                Debug.Log("在地狀態: " + isGrounded + " 玩家的Y合力狀態: " + player.CurrentVelocity.y);
                playerStateMachine.ChangeState(player.PlayerIdleState);
            }
            else
            {
                Debug.Log("在地狀態: " + isGrounded + " 玩家的Y合力狀態: " + player.CurrentVelocity.y);
                playerStateMachine.ChangeState(player.PlayerInAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }
}
