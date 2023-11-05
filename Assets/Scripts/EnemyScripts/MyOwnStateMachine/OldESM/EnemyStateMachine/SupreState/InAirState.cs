using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirState : EnemyState
{
    private float facing_direction;
    private bool is_grounded;
    private bool is_onwall;
    public bool IsJumping { get; private set; }
    private bool can_grab;
    public InAirState(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyAttribute enemyAttribute, string anim_bool_name) : base(enemy, enemyStateMachine, enemyAttribute, anim_bool_name)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        is_grounded = enemy.EnemyPhysicCheck.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();
        facing_direction = enemy.transform.localScale.x;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // if (facing_direction != 0)
        //     enemy.EnemyPhysicCheck.CheckIfNeedToTurn(facing_direction > 0);

        if (is_grounded && enemy.EnemyPhysicCheck.CurrentVelocity.y < 0.01f)
        {
            Debug.Log("敵人Y力: " + enemy.EnemyPhysicCheck.CurrentVelocity.y);
            enemyStateMachine.ChangeState(enemy.LandState);
        }
        // else if (melee_input && enemy.MeleeAttackState.CheckIfCanAttack())
        // {
        //     enemyStateMachine.ChangeState(enemy.MeleeAttackState);
        // }
        // else if (dash_input && enemy.DashState.CheckIfCanDash())
        // {
        //     playerStateMachine.ChangeState(player.PlayerDashState);
        // }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.InAirMove(1, facing_direction, enemyAttribute.MoveMaxSpeed, enemyAttribute.MoveAccelAmount * enemyAttribute.AccelInAir,
        enemyAttribute.MoveDeccelAmount * enemyAttribute.DeccelInAir, enemyAttribute.JumpHangTimeThreshold, enemyAttribute.JumpHangAccelerationMult,
        enemyAttribute.jumpHangMaxSpeedMult, enemyAttribute.DoConserveMomentum, IsJumping);
    }

    //public bool CanWallJumpCut() => player.WallJumpState.IsWallJumping && player.CurrentVelocity.y > 0;
    public bool CanJumpCut() => IsJumping && enemy.EnemyPhysicCheck.CurrentVelocity.y > 0;

    // public void SetJumping(bool setting) => IsJumping = setting;
    // public void SetJumpCut(bool setting) => IsJumpCut = setting;
    // public bool CanWallSlide() => !IsJumping && is_onwall && XInputAtWall() && enemy.CurrentVelocity.y < 0;
    // public bool CanWallGrab() => !IsJumping && is_onwall && XInputAtWall() && can_grab;
    // public bool XInputAtWall() => (enemy.LastOnWallLeftTime > 0 && x_Input < 0) || (enemy.LastOnWallRightTime > 0 && x_Input > 0);

}
