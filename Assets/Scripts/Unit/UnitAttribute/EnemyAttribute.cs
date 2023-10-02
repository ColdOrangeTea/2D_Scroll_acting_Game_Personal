using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// fileName：建立檔案時的預設名稱。menuName：選單工具的路徑。order：在選單清單中的順序
[CreateAssetMenu(fileName = "New EnemyAttribute", menuName = "DataTool/ Create EnemyAttribute Asset", order = 2)]
public class EnemyAttribute : ScriptableObject
{
    [Space(20)]
    [Header("Gravity")]

    //Downwards force (gravity) needed for the desired jumpHeight and jumpTimeToApex.
    [HideInInspector] public float GravityStrength; // 用來儲存 -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex) 的值

    // Strength of the player's gravity as a multiplier of gravity (set in ProjectSettings/Physics2D).
    // Also the value the player's rigidbody2D.gravityScale is set to.
    [HideInInspector] public float GravityScale; // 用來儲存 gravityStrength / Physics2D.gravity.y 的值

    [Tooltip("處在墜落狀態時，用來乘 gravityScale 的乘數")]
    public float FallGravityMult;

    [Tooltip("玩家墜落時的最大墜落速度（終端速度） 等等竟然用到流體力學(?!)")]
    public float MaxFallSpeed;

    [Tooltip("當玩家處在墜落狀態時按下鍵，讓玩家掉落速度增快的乘數")]
    public float FastFallGravityMult;

    [Tooltip("因應掉落速度增快情況的最大墜落速度（終端速度）")]
    public float MaxFastFallSpeed;

    [Space(20)]
    [Header("Move")]
    public float MoveDuration = 2;
    public float MoveSpeed = 0.2f;
    public float MoveMaxSpeed = 1;
    public float MoveAcceleration;
    [HideInInspector] public float MoveAccelAmount; // 用來儲存(50 * MoveAcceleration) / MoveMaxSpeed 的值
    public float MoveDecceleration;
    [HideInInspector] public float MoveDeccelAmount; // 用來儲存(50 * MoveDecceleration) / MoveMaxSpeed 的值

    [Space(5)]
    [Tooltip("處在空中時，用來乘的乘數")]
    [Range(0f, 1)] public float AccelInAir;

    [Range(0f, 1)] public float DeccelInAir;
    [Space(5)]
    public bool DoConserveMomentum = true; // 動量守恆定律

    [Space(20)]

    [Header("Jump")]

    [Tooltip("跳躍的高度")]
    public float JumpHeight;

    [Tooltip("施加跳躍力和到達所需跳躍高度之間的時間，這些值同時還控制了該物件的 gravity 和 jumpForce ")]
    public float JumpTimeToApex;

    [Tooltip("該物件跳躍時實際向上施加的力")]
    [HideInInspector] public float JumpForce; // 用來儲存 Mathf.Abs(gravityStrength) * jumpTimeToApex  的值

    [Header("Both Jumps")]

    // [Tooltip("在跳躍狀態放開按鈕的情況下增加重力的乘數")]
    // public float jumpCutGravityMult;

    [Tooltip("在接近跳躍頂點(Apex)（所需的最大高度）時減少重力")]
    [Range(0f, 1)] public float JumpHangGravityMult;

    [Tooltip("玩家將經歷到額外的 ''jump hang'' (趨近於0的)速度，The player's velocity.y 在跳躍的頂點最接近0(類似拋物線或二次函數的梯度)")]
    public float JumpHangTimeThreshold;
    [Space(0.5f)]
    public float JumpHangAccelerationMult;
    public float jumpHangMaxSpeedMult;

    [Space(20)]
    [Header("Melee State")]
    public float MeleeCooldown;
    public float MaxMeleeHoldTime;
    public float MeleeHoldtimeScale;
    public float MeleeDuration;
    public float MeleeDrag;

    [Space(20)]
    [Header("Idle State")]

    public float IdleWaitTime = 2;



    //Unity Callback, called when the inspector updates
    private void OnValidate()
    {
        // Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        GravityStrength = -(2 * JumpHeight) / (JumpTimeToApex * JumpTimeToApex);

        // Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        GravityScale = GravityStrength / Physics2D.gravity.y;

        //Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / MoveMaxSpeed
        MoveAccelAmount = (50 * MoveAcceleration) / MoveMaxSpeed;
        MoveDeccelAmount = (50 * MoveDecceleration) / MoveMaxSpeed;

        //Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        JumpForce = Mathf.Abs(GravityStrength) * JumpTimeToApex;

        #region Variable Ranges
        MoveAcceleration = Mathf.Clamp(MoveAcceleration, 0.01f, MoveMaxSpeed);
        MoveDecceleration = Mathf.Clamp(MoveDecceleration, 0.01f, MoveMaxSpeed);
        #endregion
    }
}
