using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityState : State
{
    protected bool isAbilityDone;
    protected bool isGrounded;
    protected float xInput;
    public AbilityState(Player player, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(player, stateMachine, unitAttribute, animBoolName)
    {
    }

    public override void DoChecks() //fixedupdate
    {
        base.DoChecks();

        player.OnGroundCheck();
        isGrounded = player.CheckIfGrounded();
        //Debug.Log(isGrounded);
        //Debug.Log(Time.time);
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

        xInput = player.inputHandler.XInput;

        if (isAbilityDone)
        {
            // if (isGrounded && playerMovement.inputHandler.YInput == -1)
            // {
            //     stateMachine.ChangeState(playerMovement.CrouchIdleState);
            // }
            if (isGrounded && player.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }
}
