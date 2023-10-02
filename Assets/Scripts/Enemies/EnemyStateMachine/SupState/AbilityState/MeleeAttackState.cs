using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AbilityState
{
    private bool is_holding;

    private bool punch_used;
    private Vector2 punch_direction;
    private bool punch_stop_input;
    private Vector2 punch_direction_input;
    private float last_punch_time;

    public MeleeAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyAttribute enemyAttribute, string anim_bool_name) : base(enemy, enemyStateMachine, enemyAttribute, anim_bool_name)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //CanSlash = true;
        // enemy.InputHandler.UseMeleeInput();




        is_holding = true;
        punch_used = false;

        // playerMovement.AimPivot.gameObject.SetActive(true);
        Time.timeScale = enemyAttribute.MeleeHoldtimeScale;
        enemy.RB.drag = enemyAttribute.MeleeDrag;

        startTime = Time.unscaledTime;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            if (!punch_used)
            {
                //Debug.Log("Slashed!boom");
                // enemy.AirSlash();



                punch_used = true;
            }
            //執行Slash
            //Slash時間
            // if (Time.time >= startTime + enemyAttribute.fireballDuration)
            // {
            //     isAbilityDone = true;
            //     last_punch_time = Time.time;
            //     enemy.RB.drag = 0f;
            // }




        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isGrounded)
        {
            enemy.GroundMove(1, xInput, enemyAttribute.MoveMaxSpeed, enemyAttribute.MoveAccelAmount, enemyAttribute.MoveDeccelAmount);
        }
        else
            enemy.InAirMove(1, xInput, enemyAttribute.MoveMaxSpeed, enemyAttribute.MoveAccelAmount * enemyAttribute.AccelInAir,
            enemyAttribute.MoveDeccelAmount * enemyAttribute.DeccelInAir, enemyAttribute.JumpHangTimeThreshold,
            enemyAttribute.JumpHangAccelerationMult, enemyAttribute.jumpHangMaxSpeedMult, enemyAttribute.DoConserveMomentum, false);
    }
    public bool CheckIfCanSlash()
    {
        return Time.time >= last_punch_time + enemyAttribute.MeleeCooldown;
    }
    void AimFacingDirection()
    {
        enemy.CheckDirectionToFace(punch_direction.x > 0 && punch_direction.x != 0);
    }

}
