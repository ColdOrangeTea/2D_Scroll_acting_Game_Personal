using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player player, PlayerStateMachine playerStateMachine, UnitAttribute unitAttribute, string anim_bool_name) : base(player, playerStateMachine, unitAttribute, anim_bool_name)
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
