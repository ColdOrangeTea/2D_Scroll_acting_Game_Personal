using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirState : EnemyState
{
    private float x_Input;
    private bool jump_cut_input;
    private bool jump_input;
    private bool fireball_input;
    private bool airpush_input;
    private bool melee_input;
    private bool dash_input;
    private bool is_grounded;
    private bool is_onwall;
    public bool IsJumping { get; private set; }
    public bool IsJumpCut { get; private set; }
    private bool can_grab;
    public InAirState(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyAttribute enemyAttribute, string anim_bool_name) : base(enemy, enemyStateMachine, enemyAttribute, anim_bool_name)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        is_grounded = enemy.CheckIfGrounded();

        // playerMovement.OnWallCheck();
        // isOnWall = playerMovement.CheckIfOnWall();

        //Debug.Log(isGrounded);
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

        if (x_Input != 0)
            enemy.CheckDirectionToFace(x_Input > 0);

        if (is_grounded && enemy.CurrentVelocity.y < 0.01f)
        {
            IsJumpCut = false;
            enemyStateMachine.ChangeState(enemy.LandState);
        }
        else if (melee_input && enemy.MeleeAttackState.CheckIfCanSlash())
        {
            enemyStateMachine.ChangeState(enemy.MeleeAttackState);
        }
        // else if (dash_input && enemy.DashState.CheckIfCanDash())
        // {
        //     playerStateMachine.ChangeState(player.PlayerDashState);
        // }



        CheckJumping();
        CheckJumpCut();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.InAirMove(1, x_Input, enemyAttribute.MoveMaxSpeed, enemyAttribute.MoveAccelAmount * enemyAttribute.AccelInAir,
        enemyAttribute.MoveDeccelAmount * enemyAttribute.DeccelInAir, enemyAttribute.JumpHangTimeThreshold, enemyAttribute.JumpHangAccelerationMult,
        enemyAttribute.jumpHangMaxSpeedMult, enemyAttribute.DoConserveMomentum, IsJumping);
    }

    private void CheckJumping()
    {
        if (IsJumping && enemy.CurrentVelocity.y < 0)
            IsJumping = false;
    }
    private void CheckJumpCut()
    {
        if (jump_cut_input && CanJumpCut())
        {
            IsJumpCut = true;
        }
    }

    //public bool CanWallJumpCut() => player.WallJumpState.IsWallJumping && player.CurrentVelocity.y > 0;
    public bool CanJumpCut() => IsJumping && enemy.CurrentVelocity.y > 0;

    // public void SetJumping(bool setting) => IsJumping = setting;
    // public void SetJumpCut(bool setting) => IsJumpCut = setting;
    // public bool CanWallSlide() => !IsJumping && is_onwall && XInputAtWall() && enemy.CurrentVelocity.y < 0;
    // public bool CanWallGrab() => !IsJumping && is_onwall && XInputAtWall() && can_grab;
    // public bool XInputAtWall() => (enemy.LastOnWallLeftTime > 0 && x_Input < 0) || (enemy.LastOnWallRightTime > 0 && x_Input > 0);

}
