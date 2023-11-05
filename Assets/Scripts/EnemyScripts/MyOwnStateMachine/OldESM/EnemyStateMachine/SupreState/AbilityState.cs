using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityState : EnemyState
{
    protected bool isAbilityDone;
    protected bool isGrounded;
    protected bool isSawPlayer;
    // protected float lastAbilityDoneTime;

    public AbilityState(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyAttribute enemyAttribute, string anim_bool_name) : base(enemy, enemyStateMachine, enemyAttribute, anim_bool_name)
    {
    }
    public override void DoChecks() //fixedupdate
    {
        base.DoChecks();

        isGrounded = enemy.EnemyPhysicCheck.CheckIfGrounded();
        isSawPlayer = enemy.EnemyPhysicCheck.CheckIfSawPlayer();
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
        // lastAbilityDoneTime = Time.time;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAbilityDone)
        {
            if (isGrounded)
            {
                enemyStateMachine.ChangeState(enemy.IdleState);
            }
            else
            {
                enemyStateMachine.ChangeState(enemy.InAirState);
            }


        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }
}
