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
    public AttackState(PlayerMovement playerMovement, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(playerMovement, stateMachine, unitAttribute, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        //CanSlash = true;
        playerMovement.inputHandler.UseMeleeInput();

        isHolding = true;
        slashUsed = false;

        // playerMovement.AimPivot.gameObject.SetActive(true);
        Time.timeScale = unitAttribute.slashHoldtimeScale;
        playerMovement.RB.drag = unitAttribute.SlashDrag;

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

                SlashStopInput = playerMovement.inputHandler.SlashAimStopInput;
                SlashDirectionInput = playerMovement.inputHandler.PointerDirectionInput;
                xInput = playerMovement.inputHandler.XInput;

                playerMovement.RotateAimPivot();

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

                if (playerMovement.RB.transform.localScale.x == -1)
                    angle -= 90;

                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                // playerMovement.AimPivot.rotation = rotation;
            }
            else
            {
                if (!slashUsed)
                {
                    //Debug.Log("Slashed!boom");
                    playerMovement.AirSlash();
                    slashUsed = true;
                }
                //執行Slash
                //Slash時間
                if (Time.time >= startTime + unitAttribute.fireballDuration)
                {
                    isAbilityDone = true;
                    lastSlashTime = Time.time;
                    playerMovement.RB.drag = 0f;
                }
            }
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isGrounded)
        {
            playerMovement.GroundMove(1, xInput, unitAttribute.runMaxSpeed, unitAttribute.runAccelAmount, unitAttribute.runDeccelAmount);
        }
        else
            playerMovement.InAirMove(1, xInput, unitAttribute.runMaxSpeed, unitAttribute.runAccelAmount * unitAttribute.accelInAir,
            unitAttribute.runDeccelAmount * unitAttribute.deccelInAir, unitAttribute.jumpHangTimeThreshold,
            unitAttribute.jumpHangAccelerationMult, unitAttribute.jumpHangMaxSpeedMult, unitAttribute.doConserveMomentum, false);
    }
    public bool CheckIfCanSlash()
    {
        return Time.time >= lastSlashTime + unitAttribute.SlashCooldown;
    }
    void AimFacingDirection()
    {
        playerMovement.CheckDirectionToFace(SlashDirection.x > 0 && SlashDirection.x != 0);
    }

}

// public abstract class AttackState : MonoBehaviour
// {
//     [SerializeField] protected UnitStateMachineManager unitStateMachineManager;

//     [SerializeField] protected float startUp = 0;
//     [SerializeField] protected float attackDuration = 10;

//     // 用來計時的變數，好控制攻擊間隔
//     [SerializeField] protected float attackColdDown = 19.5F;

//     [SerializeField] protected Transform attackTransform;
//     [SerializeField] protected LayerMask attackableLayer;

//     // [Space(5)]

//     // [Header("用來檢查前方是否有玩家的變數")]
//     // [SerializeField] protected Transform playerCheckTransform;
//     // [SerializeField] protected float playerCheckX;
//     // [SerializeField] protected float playerCheckY;
//     // [SerializeField] protected LayerMask playerLayer;

//     void Awake()
//     {
//         InitAttackState();
//     }

//     public abstract IEnumerator Attackperiod();
//     // public abstract bool PlayerCheck(float moveDirection);
//     public abstract IEnumerator Attacking();
//     public abstract void InitAttackState();

// }
