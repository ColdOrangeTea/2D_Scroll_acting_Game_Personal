using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// fileName：建立檔案時的預設名稱。menuName：選單工具的路徑。order：在選單清單中的順序
[CreateAssetMenu(fileName = "New PlayerAttribute", menuName = "SO/DataTool/ Create  PlayerAttribute Asset", order = 1)]
public class PlayerAttribute : ScriptableObject
{
    //     public int maxHp = 0;
    //     public int hp = 0;
    //     public int maxMp = 0;
    //     public int mp = 0;
    //     public int attack = 0;

    // [Space(20)]
    // [Header("Gravity")]

    //Downwards force (gravity) needed for the desired jumpHeight and jumpTimeToApex.
    [HideInInspector] public float GravityStrength;

    // Strength of the player's gravity as a multiplier of gravity (set in ProjectSettings/Physics2D).
    // Also the value the player's rigidbody2D.gravityScale is set to.
    [HideInInspector] public float GravityScale;
    [Space(20)]
    [Header("Gravity")]

    [Tooltip("Multiplier to the player's gravityScale when falling. 建議數值: 1.5")]
    public float FallGravityMult;

    [Tooltip("Maximum fall speed (terminal velocity) of the player when falling.建議數值: 25")]
    public float MaxFallSpeed;

    [Tooltip("Larger multiplier to the player's gravityScale when they are falling and a downwards input is pressed. Seen in games such as Celeste, lets the player fall extra fast if they wish. 建議數值: 2")]
    public float FastFallGravityMult;

    [Tooltip("Maximum fall speed(terminal velocity) of the player when performing a faster fall. 建議數值: 30")]
    public float MaxFastFallSpeed;


    [Space(20)]

    [Header("Run")]
    [Tooltip("Target speed we want the player to reach. 建議數值: 11")]
    public float RunMaxSpeed;

    [Tooltip("The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all. 建議數值: 2.5")]
    public float RunAcceleration;

    [Tooltip("The actual force (multiplied with speedDiff) applied to the player.")]
    [HideInInspector] public float RunAccelAmount;

    [Tooltip("The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all. 建議數值: 5")]
    public float RunDecceleration;

    [Tooltip("//Actual force (multiplied with speedDiff) applied to the player.")]
    [HideInInspector] public float RunDeccelAmount;

    [Space(5)]
    [Tooltip("Multipliers applied to acceleration rate when airborne. 建議數值: 0.65、0.65")]
    [Range(0f, 1)] public float AccelInAir;

    [Range(0f, 1)] public float DeccelInAir;
    [Space(5)]
    public bool DoConserveMomentum = true; // 動量守恆定律

    [Space(20)]

    [Header("Jump")]

    [Tooltip("Height of the player's jump 建議數值: 3.5")]
    public float JumpHeight;

    [Tooltip("Time between applying the jump force and reaching the desired jump height. These values also control the player's gravity and jump force. 建議數值: 0.3")]
    public float JumpTimeToApex;

    [Tooltip("//The actual force applied (upwards) to the player when they jump.")]
    [HideInInspector] public float JumpForce;

    [Header("Both Jumps")]

    [Tooltip("Multiplier to increase gravity if the player releases thje jump button while still jumping. 建議數值: 2")]
    public float JumpCutGravityMult;

    [Tooltip("Reduces gravity while close to the apex (desired max height) of the jump. 建議數值: 0.5")]
    [Range(0f, 1)] public float JumpHangGravityMult;

    [Tooltip("Speeds (close to 0) where the player will experience extra ''jump hang''. The player's velocity.y is closest to 0 at the jump's apex (think of the gradient of a parabola or quadratic function) 建議數值: 1")]
    public float JumpHangTimeThreshold;
    [Space(0.5f)]
    [Tooltip("建議數值: 1.1")]
    public float JumpHangAccelerationMult;
    [Tooltip("建議數值: 1.3")]
    public float JumpHangMaxSpeedMult;

    [Tooltip("Multipliers applied to acceleration rate when airborne. 建議數值: 0.8 、建議數值: 0.8")]
    [Range(0f, 1)] public float AccelOnGround;
    [Range(0f, 1)] public float DeccelOnGround;
    [Space(5)]
    public bool CrouchSlideDoConserveMomentum = true;

    [Space(20)]

    [Header("Assists")]

    [Tooltip("Grace period after falling off a platform, where you can still jump. coyoteTime:當玩家自地形邊界走出，發生離地的瞬間，此時角色已經底部浮空，但玩家仍可以進行跳躍的指令 建議數值: 0.1")]
    [Range(0.01f, 0.5f)] public float CoyoteTime;

    [Tooltip("Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met. 建議數值: 0.1")]
    [Range(0.01f, 0.5f)] public float JumpInputBufferTime;

    [Space(20)]

    [Header("Dash 建議數值: 1 、 建議數值: 30")]
    public int DashAmount;
    public float DashSpeed;

    [Tooltip("Duration for which the game freezes when we press dash but before we read directional input and apply a force. 建議數值: 0.05")]
    public float DashSleepTime;

    [Space(5)]
    [Tooltip("建議數值: 0.15")]
    public float DashAttackTime;
    [Space(5)]

    [Tooltip("Time after you finish the inital drag phase, smoothing the transition back to idle (or any standard state) 建議數值: 0.15")]
    public float DashEndTime;

    [Tooltip("Slows down player, makes dash feel more responsive (used in Celeste) 建議數值: 15 、建議數值: 15")]
    public Vector2 DashEndSpeed;

    [Tooltip("Unused,Slows the affect of player movement while dashing. 建議數值: 0.5")]
    [Range(0f, 1f)] public float DashEndRunLerp;

    [Space(5)]
    [Tooltip("建議數值:  0.1")]
    public float DashRefillTime;
    [Space(5)]
    [Tooltip("建議數值: 0.1")]
    [Range(0.01f, 0.5f)] public float DashInputBufferTime;


    [Header("Melee 建議數值: 0.1")]
    [Range(0.01f, 1f)] public float MeleeInputBufferTime;

    [Header("Punch State 建議數值: 0.5、 建議數值: 1 、建議數值: 0.25 、建議數值: 0.5 、建議數值: 2")]
    public float PunchCooldown;
    public float MaxPunchHoldTime;
    public float PunchHoldtimeScale;
    public float PunchDuration;
    public float PunchDrag;



    //Unity Callback, called when the inspector updates
    private void OnValidate()
    {
        #region Gravity
        //Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        GravityStrength = -(JumpHeight) / (JumpTimeToApex * JumpTimeToApex);

        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        GravityScale = GravityStrength / Physics2D.gravity.y;
        #endregion

        #region Move
        //Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        RunAccelAmount = (50 * RunAcceleration) / RunMaxSpeed;
        RunDeccelAmount = -(50 * RunDecceleration) / RunMaxSpeed;
        #endregion

        #region Jump
        //Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        JumpForce = Mathf.Abs(1.5f * GravityStrength) * JumpTimeToApex;
        #endregion

        #region Variable Ranges
        RunAcceleration = Mathf.Clamp(RunAcceleration, 0.01f, RunMaxSpeed);
        RunDecceleration = Mathf.Clamp(RunDecceleration, -10f, RunMaxSpeed);
        #endregion
    }
}
