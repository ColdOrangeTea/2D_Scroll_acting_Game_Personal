using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerState
public class PlayerState
{
    protected Core core;

    protected Player player;
    protected PlayerStateMachine playerStateMachine;
    protected PlayerAttribute playerAttribute;

    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected float startTime;

    private string anim_bool_name;


    public PlayerState(Player player, PlayerStateMachine playerStateMachine, PlayerAttribute playerAttribute, string anim_bool_name)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
        this.playerAttribute = playerAttribute;
        this.anim_bool_name = anim_bool_name;
        core = player.Core;
    }
    public virtual void Enter()
    {
        DoChecks();

        // 動畫先保留
        player.Anim.SetBool(anim_bool_name, true);

        startTime = Time.time;

        Debug.Log(anim_bool_name);
        isAnimationFinished = false;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        // 動畫先保留
        player.Anim.SetBool(anim_bool_name, false);

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
