using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerState
public class State
{
    protected Core core;

    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected UnitAttribute unitAttribute;

    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected float startTime;

    private string animBoolName;


    public State(Player player, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.unitAttribute = unitAttribute;
        this.animBoolName = animBoolName;
        core = player.Core;
    }
    public virtual void Enter()
    {
        DoChecks();

        // 動畫先保留
        // playerMovement.Anim.SetBool(animBoolName, true);

        startTime = Time.time;

        Debug.Log(animBoolName);
        isAnimationFinished = false;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        // 動畫先保留
        // playerMovement.Anim.SetBool(animBoolName, false);

        isExitingState = true;
    }

    public virtual void LogicUpdate() //Update
    {
    }

    public virtual void PhysicsUpdate() //FixedUpdate
    {
        DoChecks();
    }

    public virtual void DoChecks() //FixedUpdate
    {
    }

    public virtual void AnimationTrigger()
    {

    }
    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
