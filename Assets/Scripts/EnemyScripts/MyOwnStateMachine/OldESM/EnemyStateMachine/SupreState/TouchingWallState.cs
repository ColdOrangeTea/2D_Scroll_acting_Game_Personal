using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingWallState : EnemyState
{
    protected bool isGrounded;
    protected bool isOnWall;
    protected float xInput;
    protected bool jumpInput;
    public bool IsOnWall { get; private set; }
    public TouchingWallState(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyAttribute enemyAttribute, string anim_bool_name) : base(enemy, enemyStateMachine, enemyAttribute, anim_bool_name)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        // enemy.OnGroundCheck();
        // isGrounded = enemy.CheckIfGrounded();

        // enemy.OnWallCheck();
        // isOnWall = enemy.CheckIfOnWall();
    }

    public override void Enter()
    {
        base.Enter();

        IsOnWall = true;
    }

    public override void Exit()
    {
        base.Exit();

        IsOnWall = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // xInput = enemy.inputHandler.XInput;
        // jumpInput = enemy.inputHandler.JumpInput();

        // if(jumpInput)
        // {
        //     enemyStateMachine.ChangeState(enemy.WallJumpState);
        // }
        // else if(isGrounded)
        // {
        //     enemyStateMachine.ChangeState(enemy.IdleState);
        // }
        // else if(!OnWall())
        // {
        //     enemyStateMachine.ChangeState(enemy.InAirState);
        // }


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    // public bool OnWall() => isOnWall && XInputAtWall();
    // public bool XInputAtWall() => (enemy.LastOnWallLeftTime > 0 && xInput < 0) || (enemy.LastOnWallRightTime > 0 && xInput > 0);
}
