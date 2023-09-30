using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// fileName：建立檔案時的預設名稱。menuName：選單工具的路徑。order：在選單清單中的順序
[CreateAssetMenu(fileName = "New EnemyAttribute", menuName = "DataTool/ Create EnemyAttribute Asset", order = 2)]
public class EnemyAttribute : ScriptableObject
{
    public int maxHp = 0;
    public int hp = 0;
    public int maxMp = 0;
    public int mp = 0;
    public int attack = 0;

    // [Space(20)]
    // [Header("Gravity")]

    //Downwards force (gravity) needed for the desired jumpHeight and jumpTimeToApex.
    [HideInInspector] public float gravityStrength;

    // Strength of the player's gravity as a multiplier of gravity (set in ProjectSettings/Physics2D).
    // Also the value the player's rigidbody2D.gravityScale is set to.
    [HideInInspector] public float gravityScale;
    [Space(20)]
    [Header("Gravity")]

    [Tooltip("Multiplier to the player's gravityScale when falling.")]
    public float fallGravityMult;

    [Tooltip("Maximum fall speed (terminal velocity) of the player when falling.")]
    public float maxFallSpeed;

    [Tooltip("Larger multiplier to the player's gravityScale when they are falling and a downwards input is pressed. Seen in games such as Celeste, lets the player fall extra fast if they wish.")]
    public float fastFallGravityMult;

    [Tooltip("Maximum fall speed(terminal velocity) of the player when performing a faster fall.")]
    public float maxFastFallSpeed;


    [Space(20)]

    [Header("Run")]
    [Tooltip("Target speed we want the player to reach.")]
    public float runMaxSpeed;

    [Tooltip("The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all")]
    public float runAcceleration;

    [Tooltip("//The actual force (multiplied with speedDiff) applied to the player.")]
    [HideInInspector] public float runAccelAmount;

    [Tooltip("The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all")]
    public float runDecceleration;

    [Tooltip("//Actual force (multiplied with speedDiff) applied to the player .")]
    [HideInInspector] public float runDeccelAmount;

    [Space(5)]
    [Tooltip("Multipliers applied to acceleration rate when airborne.")]
    [Range(0f, 1)] public float accelInAir;

    [Range(0f, 1)] public float deccelInAir;
    [Space(5)]
    public bool doConserveMomentum = true;

    [Space(20)]

    [Header("Jump")]

    [Tooltip("Height of the player's jump")]
    public float jumpHeight;

    [Tooltip("Time between applying the jump force and reaching the desired jump height. These values also control the player's gravity and jump force.")]
    public float jumpTimeToApex;

    [Tooltip("//The actual force applied (upwards) to the player when they jump.")]
    [HideInInspector] public float jumpForce;

    [Header("Both Jumps")]

    [Tooltip("Multiplier to increase gravity if the player releases thje jump button while still jumping")]
    public float jumpCutGravityMult;

    [Tooltip("Reduces gravity while close to the apex (desired max height) of the jump")]
    [Range(0f, 1)] public float jumpHangGravityMult;

    [Tooltip("Speeds (close to 0) where the player will experience extra ''jump hang''. The player's velocity.y is closest to 0 at the jump's apex (think of the gradient of a parabola or quadratic function)")]
    public float jumpHangTimeThreshold;
    [Space(0.5f)]
    public float jumpHangAccelerationMult;
    public float jumpHangMaxSpeedMult;

    [Header("Wall Jump")]
    [Tooltip("The actual force (this time set by us) applied to the player when wall jumping.")]
    public Vector2 wallJumpForce;
    [Space(5)]

    [Tooltip("Reduces the effect of player's movement while wall jumping.")]
    [Range(0f, 1f)] public float wallJumpRunLerp;

    [Tooltip("Time after wall jumping the player's movement is slowed for.")]
    [Range(0f, 1.5f)] public float wallJumpTime;

    [Tooltip("Player will rotate to face wall jumping direction")]
    public bool doTurnOnWallJump;

    [Space(20)]

    [Header("Slide")]
    public float slideSpeed;
    public float slideAccel;

    [Space(20)]

    [Header("CrouchSlide")]
    public float crouchSlideLerp;
    public float crouchSlideSlopeLerp;

    [Space(5)]
    [Tooltip("Target speed we want the player to reach.")]
    public float slopeSlideMaxSpeed;

    [Tooltip("The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all")]
    public float crouchSlideAcceleration;

    [HideInInspector] public float crouchSlideAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.

    [Tooltip("The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all")]
    public float crouchSlideDecceleration;

    [Tooltip("//Actual force (multiplied with speedDiff) applied to the player .")]
    [HideInInspector] public float crouchSlideDeccelAmount;
    [Space(5)]

    [Tooltip("//Multipliers applied to acceleration rate when airborne.")]
    [Range(0f, 1)] public float accelOnGround;
    [Range(0f, 1)] public float deccelOnGround;
    [Space(5)]
    public bool crouchSlideDoConserveMomentum = true;

    [Space(20)]

    [Header("Assists")]

    [Tooltip("Grace period after falling off a platform, where you can still jump. coyoteTime:當玩家自地形邊界走出，發生離地的瞬間，此時角色已經底部浮空，但玩家仍可以進行跳躍的指令")]
    [Range(0.01f, 0.5f)] public float coyoteTime;

    [Tooltip("Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met.")]
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime;

    [Space(20)]

    [Header("Dash")]
    public int dashAmount;
    public float dashSpeed;

    [Tooltip("Duration for which the game freezes when we press dash but before we read directional input and apply a force")]
    public float dashSleepTime;

    [Space(5)]
    public float dashAttackTime;
    [Space(5)]

    [Tooltip("Time after you finish the inital drag phase, smoothing the transition back to idle (or any standard state)")]
    public float dashEndTime;

    [Tooltip("Slows down player, makes dash feel more responsive (used in Celeste)")]
    public Vector2 dashEndSpeed;

    [Tooltip("Unused,Slows the affect of player movement while dashing")]
    [Range(0f, 1f)] public float dashEndRunLerp;

    [Space(5)]
    public float dashRefillTime;
    [Space(5)]
    [Range(0.01f, 0.5f)] public float dashInputBufferTime;

    [Space(20)]

    [Header("Push")]
    public int pushAmount;
    public int pushForce;
    public int pushKnockbackForce;
    [Space(5)]
    public float pushRefillTime;
    [Range(0.01f, 0.1f)] public float pushInputBufferTime;

    [Space(20)]

    [Header("Fireball State")]
    [Range(0.01f, 0.1f)] public float fireballInputBufferTime;
    public float fireballCooldown;
    public float maxHoldTime;
    public float fireballHoldtimeScale;
    public float fireballDuration;
    public float fireballDrag;

    [Space(20)]

    [Header("Air Push State")]
    [Range(0.01f, 0.1f)] public float airPushInputBufferTime;
    public float airPushCooldown;
    public float airPushTime;
    public float airPushDuration;
    public float airPushDrag;

    [Space(20)]

    [Header("Melee")]
    [Range(0.01f, 0.1f)] public float meleeInputBufferTime;

    [Header("Sneak Strike State")]
    /// <summary>
    /// CD:
    /// </summary>
    public float sneakStrikeCooldown;
    public float sneakStrikeTime;
    public float sneakStrikeDuration;
    public float sneakStrikeDrag;
    [Space(20)]

    [Header("Slash State")]
    public float SlashCooldown;
    public float maxSlashHoldTime;
    public float slashHoldtimeScale;
    public float SlashDuration;
    public float SlashDrag;



    //Unity Callback, called when the inspector updates
    private void OnValidate()
    {
        //Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;

        //Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        crouchSlideAccelAmount = (50 * crouchSlideAcceleration) / slopeSlideMaxSpeed;
        crouchSlideDeccelAmount = (50 * crouchSlideDecceleration) / slopeSlideMaxSpeed;

        //Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

        #region Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);

        crouchSlideAcceleration = Mathf.Clamp(crouchSlideAcceleration, 0.01f, slopeSlideMaxSpeed);
        crouchSlideDecceleration = Mathf.Clamp(crouchSlideDecceleration, 0.01f, slopeSlideMaxSpeed);
        #endregion
    }
}
