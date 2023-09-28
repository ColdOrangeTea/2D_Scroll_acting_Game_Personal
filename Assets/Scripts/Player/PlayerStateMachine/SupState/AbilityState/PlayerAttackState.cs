using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    //public bool CanSlash { get; private set; }
    private bool is_holding;

    private bool slash_used;
    private Vector2 slash_direction;
    private bool slash_stop_input;
    private Vector2 slash_direction_input;
    private float last_slash_time;
    public PlayerAttackState(Player player, PlayerStateMachine playerStateMachine, UnitAttribute unitAttribute, string anim_bool_name) : base(player, playerStateMachine, unitAttribute, anim_bool_name)
    {
    }
    public override void Enter()
    {
        base.Enter();

        //CanSlash = true;
        player.InputHandler.UseMeleeInput();

        is_holding = true;
        slash_used = false;

        // playerMovement.AimPivot.gameObject.SetActive(true);
        Time.timeScale = unitAttribute.slashHoldtimeScale;
        player.RB.drag = unitAttribute.SlashDrag;

        startTime = Time.unscaledTime;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            if (is_holding)
            {
                // 動畫先保留
                // playerMovement.Anim.speed = 0;

                slash_stop_input = player.InputHandler.SlashAimStopInput;
                slash_direction_input = player.InputHandler.PointerDirectionInput;
                xInput = player.InputHandler.XInput;

                player.RotateAimPivot();

                if (slash_direction_input != Vector2.zero)
                {
                    slash_direction = slash_direction_input;
                }

                AimFacingDirection();

                if (slash_stop_input || Time.unscaledTime >= startTime + unitAttribute.maxHoldTime)
                {
                    // 動畫先保留
                    //按住(瞄準時間結束)
                    // playerMovement.Anim.speed = 1;

                    is_holding = false;
                    Time.timeScale = 1f;
                    startTime = Time.time;

                    // playerMovement.AimPivot.gameObject.SetActive(false);
                    //trun

                }

                float angle = Mathf.Atan2(slash_direction.y, slash_direction.x) * Mathf.Rad2Deg;

                if (player.RB.transform.localScale.x == -1)
                    angle -= 90;

                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                // playerMovement.AimPivot.rotation = rotation;
            }
            else
            {
                if (!slash_used)
                {
                    //Debug.Log("Slashed!boom");
                    player.AirSlash();
                    slash_used = true;
                }
                //執行Slash
                //Slash時間
                if (Time.time >= startTime + unitAttribute.fireballDuration)
                {
                    isAbilityDone = true;
                    last_slash_time = Time.time;
                    player.RB.drag = 0f;
                }
            }
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isGrounded)
        {
            player.GroundMove(1, xInput, unitAttribute.runMaxSpeed, unitAttribute.runAccelAmount, unitAttribute.runDeccelAmount);
        }
        else
            player.InAirMove(1, xInput, unitAttribute.runMaxSpeed, unitAttribute.runAccelAmount * unitAttribute.accelInAir,
            unitAttribute.runDeccelAmount * unitAttribute.deccelInAir, unitAttribute.jumpHangTimeThreshold,
            unitAttribute.jumpHangAccelerationMult, unitAttribute.jumpHangMaxSpeedMult, unitAttribute.doConserveMomentum, false);
    }
    public bool CheckIfCanSlash()
    {
        return Time.time >= last_slash_time + unitAttribute.SlashCooldown;
    }
    void AimFacingDirection()
    {
        player.CheckDirectionToFace(slash_direction.x > 0 && slash_direction.x != 0);
    }

}
