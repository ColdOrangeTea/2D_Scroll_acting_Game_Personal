using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : AbilityState
{
    //public bool CanSlash { get; private set; }
    private bool isHolding;

    private bool slashUsed;
    private Vector2 SlashDirection;
    private bool SlashStopInput;
    private Vector2 SlashDirectionInput;
    private float lastSlashTime;
    public AttackState(Player player, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(player, stateMachine, unitAttribute, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        //CanSlash = true;
        player.inputHandler.UseMeleeInput();

        isHolding = true;
        slashUsed = false;

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
            if (isHolding)
            {
                // 動畫先保留
                // playerMovement.Anim.speed = 0;

                SlashStopInput = player.inputHandler.SlashAimStopInput;
                SlashDirectionInput = player.inputHandler.PointerDirectionInput;
                xInput = player.inputHandler.XInput;

                player.RotateAimPivot();

                if (SlashDirectionInput != Vector2.zero)
                {
                    SlashDirection = SlashDirectionInput;
                }

                AimFacingDirection();

                if (SlashStopInput || Time.unscaledTime >= startTime + unitAttribute.maxHoldTime)
                {
                    // 動畫先保留
                    //按住(瞄準時間結束)
                    // playerMovement.Anim.speed = 1;

                    isHolding = false;
                    Time.timeScale = 1f;
                    startTime = Time.time;

                    // playerMovement.AimPivot.gameObject.SetActive(false);
                    //trun

                }

                float angle = Mathf.Atan2(SlashDirection.y, SlashDirection.x) * Mathf.Rad2Deg;

                if (player.RB.transform.localScale.x == -1)
                    angle -= 90;

                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                // playerMovement.AimPivot.rotation = rotation;
            }
            else
            {
                if (!slashUsed)
                {
                    //Debug.Log("Slashed!boom");
                    player.AirSlash();
                    slashUsed = true;
                }
                //執行Slash
                //Slash時間
                if (Time.time >= startTime + unitAttribute.fireballDuration)
                {
                    isAbilityDone = true;
                    lastSlashTime = Time.time;
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
        return Time.time >= lastSlashTime + unitAttribute.SlashCooldown;
    }
    void AimFacingDirection()
    {
        player.CheckDirectionToFace(SlashDirection.x > 0 && SlashDirection.x != 0);
    }

}
