using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunchState : PlayerAbilityState
{
    //public bool CanSlash { get; private set; }
    private bool is_holding;

    private bool punch_used;
    private Vector2 punch_direction;
    private bool punch_stop_input;
    private Vector2 punch_direction_input;
    private float last_punch_time;

    public PlayerPunchState(Player player, PlayerStateMachine playerStateMachine, UnitAttribute unitAttribute, string anim_bool_name) : base(player, playerStateMachine, unitAttribute, anim_bool_name)
    {
    }
    public override void Enter()
    {
        base.Enter();

        //CanSlash = true;
        player.InputHandler.UseMeleeInput();

        is_holding = true;
        punch_used = false;

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

                punch_stop_input = player.InputHandler.SlashAimStopInput;
                punch_direction_input = player.InputHandler.PointerDirectionInput;
                xInput = player.InputHandler.XInput;

                player.RotateAimPivot();

                if (punch_direction_input != Vector2.zero)
                {
                    punch_direction = punch_direction_input;
                }

                AimFacingDirection();

                if (punch_stop_input || Time.unscaledTime >= startTime + unitAttribute.maxHoldTime)
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

                float angle = Mathf.Atan2(punch_direction.y, punch_direction.x) * Mathf.Rad2Deg;

                if (player.RB.transform.localScale.x == -1)
                    angle -= 90;

                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                // playerMovement.AimPivot.rotation = rotation;
            }
            else
            {
                if (!punch_used)
                {
                    //Debug.Log("Slashed!boom");
                    player.AirSlash();
                    punch_used = true;
                }
                //執行Slash
                //Slash時間
                if (Time.time >= startTime + unitAttribute.fireballDuration)
                {
                    isAbilityDone = true;
                    last_punch_time = Time.time;
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
        return Time.time >= last_punch_time + unitAttribute.SlashCooldown;
    }
    void AimFacingDirection()
    {
        player.CheckDirectionToFace(punch_direction.x > 0 && punch_direction.x != 0);
    }

}
