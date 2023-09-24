using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    public DeadState(PlayerMovement playerMovement, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(playerMovement, stateMachine, unitAttribute, animBoolName)
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
        base.Enter();
    }
    public override void LogicUpdate() //Update
    {
    }

    public override void PhysicsUpdate() //FixedUpdate
    {
        DoChecks();
    }

    public override void AnimationTrigger()
    {

    }
    public override void AnimationFinishTrigger() => isAnimationFinished = true;

}
