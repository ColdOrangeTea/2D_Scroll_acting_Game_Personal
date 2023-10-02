using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyState
{
    protected Core core;

    protected Enemy enemy;
    protected EnemyStateMachine enemyStateMachine;
    protected EnemyAttribute enemyAttribute;

    protected bool isAnimationFinished;
    protected bool isExitingState;
    protected float startTime;

    private string anim_bool_name;

    public EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyAttribute enemyAttribute, string anim_bool_name)
    {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
        this.enemyAttribute = enemyAttribute;
        this.anim_bool_name = anim_bool_name;
        core = enemy.Core;
    }

    public virtual void Enter()
    {
        DoChecks();
        enemy.Anim.SetBool(anim_bool_name, true);

        startTime = Time.time;

        Debug.Log(anim_bool_name);
        isAnimationFinished = false;
        isExitingState = false;
    }
    public virtual void Exit()
    {
        enemy.Anim.SetBool(anim_bool_name, false);
        isExitingState = true;
    }
    public virtual void LogicUpdate() // Update 
    {

    }
    public virtual void PhysicsUpdate() // FixedUpdate 
    {
        DoChecks();
    }

    public virtual void DoChecks() //FixedUpdate 放在Enter、PhysicsUpdate中
    {
    }

    public virtual void AnimationTrigger()
    {

    }
    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
