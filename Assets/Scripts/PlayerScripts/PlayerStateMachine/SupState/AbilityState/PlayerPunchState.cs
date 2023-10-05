using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunchState : PlayerAbilityState
{
    private bool punch_used;
    private Vector2 punch_direction;
    private bool punch_stop_input;
    private Vector2 punch_direction_input;
    private float last_punch_time;
    // private bool can_punch;

    public PlayerPunchState(Player player, PlayerStateMachine playerStateMachine, PlayerAttribute playerAttribute, string anim_bool_name) : base(player, playerStateMachine, playerAttribute, anim_bool_name)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseMeleeInput();

        // is_holding = true;
        punch_used = false;

        // playerMovement.AimPivot.gameObject.SetActive(true);
        Time.timeScale = playerAttribute.PunchHoldtimeScale;
        player.PlayerPhysicCheck.RB.drag = playerAttribute.PunchDrag;

        startTime = Time.unscaledTime;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {

            if (CheckIfCanPunch())
            {
                Debug.Log("可以攻擊");
                player.Anim.speed = 0;
                punch_stop_input = player.InputHandler.MeleeStopInput;
                punch_direction_input = player.InputHandler.PointerDirectionInput;

                // 動作結束
                if (punch_stop_input || Time.unscaledTime >= startTime + playerAttribute.MaxHoldTime)
                {
                    Debug.Log("毆打動畫結束!!!");
                    player.Anim.speed = 1;
                    Time.timeScale = 1f;

                    startTime = Time.time;
                    last_punch_time = Time.time;
                    isAbilityDone = true;
                }
                // 動作開始
                if (!punch_used)
                {
                    Debug.Log("毆打!!!");
                    player.PlayerPhysicCheck.CheckHittedUnit();
                    punch_used = true;
                }
            }

        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isGrounded)
        {
            player.GroundMove(1, xInput, playerAttribute.RunMaxSpeed, playerAttribute.RunAccelAmount, playerAttribute.RunDeccelAmount);
        }
        else
            player.InAirMove(1, xInput, playerAttribute.RunMaxSpeed, playerAttribute.RunAccelAmount * playerAttribute.AccelInAir,
            playerAttribute.RunDeccelAmount * playerAttribute.DeccelInAir, playerAttribute.JumpHangTimeThreshold,
            playerAttribute.JumpHangAccelerationMult, playerAttribute.JumpHangMaxSpeedMult, playerAttribute.DoConserveMomentum, false);
    }
    public bool CheckIfCanPunch()
    {
        if (last_punch_time == 0) // 時間 = 0 代表初次觸發攻擊
            return true;
        else
            return Time.time >= last_punch_time + playerAttribute.PunchCooldown;
    }


    // void AimFacingDirection()
    // {
    //     player.CheckDirectionToFace(punch_direction.x > 0 && punch_direction.x != 0);
    // }

}