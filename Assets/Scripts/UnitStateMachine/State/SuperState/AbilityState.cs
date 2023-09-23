using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityState : State
{
    protected bool isAbilityDone;
    protected bool isGrounded;
    protected float xInput;
    public AbilityState(PlayerMovement playerMovement, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(playerMovement, stateMachine, unitAttribute, animBoolName)
    {
    }

    public override void DoChecks() //fixedupdate
    {
        base.DoChecks();

        playerMovement.OnGroundCheck();
        isGrounded = playerMovement.CheckIfGrounded();
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

        xInput = playerMovement.inputHandler.XInput;

        if (isAbilityDone)
        {
            // if (isGrounded && playerMovement.inputHandler.YInput == -1)
            // {
            //     stateMachine.ChangeState(playerMovement.CrouchIdleState);
            // }
            if (isGrounded && playerMovement.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(playerMovement.IdleState);
            }
            else
            {
                stateMachine.ChangeState(playerMovement.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }
}
