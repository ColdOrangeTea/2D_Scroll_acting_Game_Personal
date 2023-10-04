using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : EnemyState
{
    // protected float xInput;
    // protected float yInput;
    // protected bool jumpInput;
    // protected bool fireballInput;
    // protected bool airPushInput;
    // protected bool meleeInput;
    // protected bool dashInput;
    protected bool isGrounded;
    protected bool isSawPlayer;

    public GroundState(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyAttribute enemyAttribute, string anim_bool_name) : base(enemy, enemyStateMachine, enemyAttribute, anim_bool_name)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = enemy.EnemyPhysicCheck.CheckIfGrounded();
        isSawPlayer = enemy.EnemyPhysicCheck.CheckIfSawPlayer();
        // Debug.Log("看到玩家沒: " + isSawPlayer);
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

        if (!isGrounded)
        {
            enemyStateMachine.ChangeState(enemy.InAirState);
        }
        if (isSawPlayer)
        {
            // Debug.Log("看到玩家了!!!!!!!!!!!!!!!!!!!! " + isSawPlayer);
            enemyStateMachine.ChangeState(enemy.MeleeAttackState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
