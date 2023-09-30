using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Player : MonoBehaviour
{
    #region STATE VARIABLES
    public Core Core { get; private set; }
    public PlayerStateMachine PlayerStateMachine { get; private set; }
    public PlayerIdleState PlayerIdleState { get; private set; }
    public PlayerMoveState PlayerMoveState { get; private set; }
    public PlayerJumpState PlayerJumpState { get; private set; }
    public PlayerAttackState PlayerAttackState { get; private set; }
    public PlayerDashState PlayerDashState { get; private set; }
    public PlayerInAirState PlayerInAirState { get; private set; }
    public PlayerLandState PlayerLandState { get; private set; }
    // public WallSlideState WallSlideState { get; private set; }
    // public WallGrabState WallGrabState { get; private set; }
    // public WallJumpState WallJumpState { get; private set; }
    // public CrouchIdleState CrouchIdleState { get; private set; }
    // public SneakStrikeState SneakStrikeState { get; private set; }
    // public CrouchSlideState CrouchSlideState { get; private set; }
    // public FireballState FireballState { get; private set; }
    // public AirPushState AirPushState { get; private set; }
    // public SlashState SlashState { get; private set; }

    [SerializeField]
    private UnitAttribute unit_attribute;

    #endregion

    #region COMPONENTS

    // 動畫先保留
    // public Animator Anim { get; private set; }


    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    //public Transform FireballDirectionIndicator { get; private set; }
    // public Transform AimPivot { get; private set; }
    // public Transform FirePoint;
    // public GameObject BulletPreFab;


    #endregion

    #region CHECK PARAMETERS
    //Set all of these up in the inspector
    [Header("Checks")]
    [SerializeField] private Transform ground_checkpoint;
    //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
    [SerializeField] private Vector2 ground_checkSize = new Vector2(0.49f, 0.03f);
    [Space(5)]
    [SerializeField] private Transform attack_point;
    [SerializeField] private float slash_radius = 3f;

    [Space(5)]

    #endregion

    #region LAYERS
    [Header("Layers")]
    [SerializeField] private LayerMask ground_layer;
    [SerializeField] private LayerMask enemy_layer;

    #endregion

    #region TIMERS
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallTime { get; private set; }

    #endregion

    #region OTHER VARIABLES
    public bool IsFacingRight { get; private set; }
    public int FacingDirection { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }


    #endregion



    #region UNITY CALLBACK FUNCTIONS

    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        PlayerStateMachine = new PlayerStateMachine();

        PlayerIdleState = new PlayerIdleState(this, PlayerStateMachine, unit_attribute, "idle");
        PlayerMoveState = new PlayerMoveState(this, PlayerStateMachine, unit_attribute, "move");
        PlayerJumpState = new PlayerJumpState(this, PlayerStateMachine, unit_attribute, "jump");
        PlayerInAirState = new PlayerInAirState(this, PlayerStateMachine, unit_attribute, "inAir");
        PlayerLandState = new PlayerLandState(this, PlayerStateMachine, unit_attribute, "land");

        PlayerAttackState = new PlayerAttackState(this, PlayerStateMachine, unit_attribute, "slash");
        PlayerDashState = new PlayerDashState(this, PlayerStateMachine, unit_attribute, "dash");
    }

    private void Start()
    {

        // Anim = GetComponent<Animator>();

        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();

        PlayerStateMachine.Initialize(PlayerIdleState);
        IsFacingRight = true;
        FacingDirection = 1;
        SetGravityScale(unit_attribute.gravityScale);
    }

    private void Update()
    {
        PlayerStateMachine.CurrentState.LogicUpdate();

        CurrentVelocity = RB.velocity;
        FacingDirection = (int)transform.localScale.x;

        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;
        //OnGroundCheck();
        Gravity();
    }
    private void FixedUpdate()
    {

        PlayerStateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion

    #region MOVE METHOD

    public void GroundMove(float lerpAmount, float xInput, float maxSpeed, float accel, float deccel)
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = xInput * maxSpeed;

        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate

        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : deccel;

        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - RB.velocity.x;

        //Calculate force along x-axis to apply to thr player
        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }

    public void InAirMove(float lerpAmount, float xInput, float maxSpeed, float accel, float deccel, float jumpHangTimeThreshold, float jumpHangAccelerationMult, float jumpHangMaxSpeedMult, bool doConserveMomentum, bool isAnyJumping)
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = xInput * maxSpeed;

        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate

        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : deccel;

        #endregion

        #region Add Bonus Jump Apex Acceleration

        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if (isAnyJumping && Mathf.Abs(RB.velocity.y) < jumpHangTimeThreshold)
        {
            accelRate *= jumpHangAccelerationMult;
            targetSpeed *= jumpHangMaxSpeedMult;
        }
        #endregion

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - RB.velocity.x;

        //Calculate force along x-axis to apply to thr player
        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }
    public void Move(float lerpAmount)
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = InputHandler.XInput * unit_attribute.runMaxSpeed;
        //Debug.Log(targetSpeed);

        /*
		if(IsCrouching)
			targetSpeed = 0f;
		//We can reduce are control using Lerp() this smooths changes to are direction and speed
		if(LastOnWindTime < 0)
		{
			targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);
		}
        */
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);


        #region Calculate AccelRate
        float accelRate;


        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.

        if (LastOnGroundTime > 0) //LastOnGroundTime > 0
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? unit_attribute.runAccelAmount : unit_attribute.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? unit_attribute.runAccelAmount * unit_attribute.accelInAir : unit_attribute.runDeccelAmount * unit_attribute.deccelInAir;

        #endregion



        #region Add Bonus Jump Apex Acceleration
        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((PlayerInAirState.IsJumping) && Mathf.Abs(RB.velocity.y) < unit_attribute.jumpHangTimeThreshold)
        {
            accelRate *= unit_attribute.jumpHangAccelerationMult;
            targetSpeed *= unit_attribute.jumpHangMaxSpeedMult;
        }
        #endregion


        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (unit_attribute.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        /*
		if(playerData.doConserveMomentum && LastOnWindTime > 0 && inHorizontalWind)
			accelRate = 0.2f;
		*/
        #endregion


        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - RB.velocity.x;
        //Calculate force along x-axis to apply to thr player

        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        /*
		if(isOnSlope)
		{
			RB.AddForce(movement * GetSlopeMoveDirection(), ForceMode2D.Force);
		}
		else
			RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

		
		 * For those interested here is what AddForce() will do
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    public void CrouchSlideMove(float lerpAmount)
    {
        //Calculate the direction we want to move in and our desired velocity

        //在斜坡上會改變
        float targetSpeed = 0;

        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);


        #region Calculate AccelRate
        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.

        //if在斜坡上
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? unit_attribute.crouchSlideAccelAmount : unit_attribute.crouchSlideDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? unit_attribute.crouchSlideAccelAmount * unit_attribute.accelOnGround : unit_attribute.crouchSlideDeccelAmount * unit_attribute.deccelOnGround;

        #endregion

        //斜坡上保留速度
        /*
		#region Conserve Momentum

		//We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
		if(playerData.crouchSlideDoConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
		{
			//Prevent any deceleration from happening, or in other words conserve are current momentum
			//You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
			accelRate = 0;
		}
		*/
        /*
		if(playerData.doConserveMomentum && LastOnWindTime > 0 && inHorizontalWind)
			accelRate = 0.2f;
		
		#endregion
		*/


        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - RB.velocity.x;
        //Calculate force along x-axis to apply to thr player

        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        /*
		if(isOnSlope)
		{
			RB.AddForce(movement * GetSlopeMoveDirection(), ForceMode2D.Force);
		}
		else
			RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

		
		 * For those interested here is what AddForce() will do
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
    public void CrouchIdleMove()
    {
        RB.velocity = new Vector2(0, RB.velocity.y);
    }

    #endregion

    #region JUMP METHOD
    public void Jump()
    {
        //Ensures we can't call Jump multiple times from one press
        LastOnGroundTime = 0;

        #region Perform Jump
        //We increase the force applied if we are falling
        //This means we'll always feel like we jump the same amount 
        //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
        float force = unit_attribute.jumpForce;
        /*
		if(LastOnMudTime > 0)
			force = Data.jumpForce / 2f;
		*/

        if (RB.velocity.y < 0)
            force -= RB.velocity.y;
        if (RB.velocity.y > 0)
            RB.velocity = new Vector2(RB.velocity.x, 0);

        //CreateJumpDust();

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        #endregion
    }

    public void WallJump()
    {
        //Ensures we can't call Wall Jump multiple times from one press
        LastOnGroundTime = 0;

        float lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;

        LastOnWallRightTime = 0;
        LastOnWallLeftTime = 0;

        #region Perform Wall Jump

        Vector2 force = new Vector2(unit_attribute.wallJumpForce.x, unit_attribute.wallJumpForce.y);
        force.x *= lastWallJumpDir; //apply force in opposite direction of wall

        if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
            force.x -= RB.velocity.x;

        if (RB.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
            force.y -= RB.velocity.y;

        //Unlike in the run we want to use the Impulse mode.
        //The default mode will apply are force instantly ignoring masss

        //CreateJumpDust();

        RB.AddForce(force, ForceMode2D.Impulse);
        #endregion
    }

    #endregion

    #region WALL SLIDE METHOD
    public void WallSlide()
    {
        //Works the same as the Run but only in the y-axis
        //THis seems to work fine, buit maybe you'll find a better way to implement a slide into this system
        //Debug.Log(RB.velocity);
        if (RB.velocity.y >= unit_attribute.slideSpeed)
        {
            RB.velocity = new Vector2(RB.velocity.x, RB.velocity.y - 1);
        }
        float speedDif = unit_attribute.slideSpeed - RB.velocity.y;
        float movement = Mathf.Abs(speedDif * unit_attribute.slideAccel);
        //So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
        //The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        RB.AddForce(movement * Vector2.down);
    }
    public void Grab()
    {
        RB.velocity = Vector2.zero;
    }
    #endregion

    #region CHECK METHODS

    #region GROUND METHOD
    public bool CheckIfGrounded()
    {
        return LastOnGroundTime > 0;
    }
    public void OnGroundCheck()
    {
        if (Physics2D.OverlapBox(ground_checkpoint.position, ground_checkSize, 0, ground_layer)) //checks if set box overlaps with ground
        {
            //Debug.Log("ground");
            //if so sets the lastGrounded to coyoteTime  coyoteTime:當玩家自地形邊界走出，發生離地的瞬間，此時角色已經底部浮空，但玩家仍可以進行跳躍的指令。
            LastOnGroundTime = unit_attribute.coyoteTime;
        }
    }
    #endregion

    #region SLOPE

    #endregion


    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }

    #endregion

    #region GRAVITY

    private void Gravity()
    {

        if (PlayerInAirState.IsJumpCut)
        {
            //Higher gravity if jump button released
            //Debug.Log("cutcut");
            SetGravityScale(unit_attribute.gravityScale * unit_attribute.jumpCutGravityMult);
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -unit_attribute.maxFallSpeed));
        }
        else if ((PlayerInAirState.IsJumping) && Mathf.Abs(RB.velocity.y) < unit_attribute.jumpHangTimeThreshold)
        {
            SetGravityScale(unit_attribute.gravityScale * unit_attribute.jumpHangGravityMult);
        }
        else if (RB.velocity.y < 0f && LastOnGroundTime < 0)
        {
            //Higher gravity if falling
            SetGravityScale(unit_attribute.gravityScale * unit_attribute.fallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -unit_attribute.maxFallSpeed));
        }
        else
        {
            //Debug.Log("normal");
            SetGravityScale(unit_attribute.gravityScale);
        }
    }

    #endregion

    #region OTHER METHODS

    private void AnimationTrigger() => PlayerStateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => PlayerStateMachine.CurrentState.AnimationFinishTrigger();

    public void Turn()
    {
        //stores scale and flips the player along the x axis, 

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        IsFacingRight = !IsFacingRight;

        /*
		if(!IsDashing)
		{
			Vector3 scale = transform.localScale; 
			scale.x *= -1;
			transform.localScale = scale;

			IsFacingRight = !IsFacingRight;
		}
        */
    }
    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }

    #endregion

    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(ground_checkpoint.position, ground_checkSize);
        Gizmos.color = Color.blue;

        Gizmos.color = Color.red;

        //Gizmos.color=Color.white;
        //Gizmos.DrawSphere(_slashPoint.position,_slashRadius);//3Dball WTF!!
        //Gizmos.DrawWireCube(_barrelCheckPoint.position, _pushCheckSize);
        //Gizmos.DrawRay(_groundCheckPoint.position , Vector2.down * slopeRaycastDistance);
    }
    #endregion

    #region PLAYER Functions
    [Space(5)][Range(0, 50)] public float ProjectileSpeed = 20f;
    public void FireProjectile()
    {
        // GameObject BulletIns = Instantiate(BulletPreFab, FirePoint.position, AimPivot.transform.rotation);
        // BulletIns.GetComponent<Rigidbody2D>().velocity = AimPivot.right * ProjectileSpeed;
    }


    public void AirSlash()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attack_point.position, slash_radius, enemy_layer);
        foreach (Collider2D Enemy in hitEnemies)
        {
            Debug.Log(Enemy.name);
            //doDMG
        }
    }
    public void RotateAimPivot()
    {
        Vector2 dir = InputHandler.RawPointerDirectionInput;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (RB.transform.localScale.x == -1)
            angle += 180;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // AimPivot.rotation = rotation;
    }
    public void Sleep(float duration)
    {
        //Method used so we don't need to call StartCoroutine everywhere
        //nameof() notation means we don't need to input a string directly.
        //Removes chance of spelling mistakes and will improve error messages if any
        StartCoroutine(nameof(PerformSleep), duration);
    }
    private IEnumerator PerformSleep(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration); //Must be Realtime since timeScale with be 0 
        Time.timeScale = 1;
    }
    public void GoDash(Vector2 dir)
    {
        StartCoroutine(nameof(StartDash), dir);
    }
    private IEnumerator StartDash(Vector2 dir)
    {
        //Overall this method of dashing aims to mimic Celeste, if you're looking for
        // a more physics-based approach try a method similar to that used in the jump
        float startTime = Time.time;
        //_isDashAttacking = true;

        SetGravityScale(0);

        //We keep the player's velocity at the dash speed during the "attack" phase (in celeste the first 0.15s)
        while (Time.time - startTime <= unit_attribute.dashAttackTime)
        {
            RB.velocity = dir.normalized * unit_attribute.dashSpeed;
            //Pauses the loop until the next frame, creating something of a Update loop. 
            //This is a cleaner implementation opposed to multiple timers and this coroutine approach is actually what is used in Celeste :D
            yield return null;
        }
        startTime = Time.time;
        //Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
        SetGravityScale(unit_attribute.gravityScale);
        RB.velocity = unit_attribute.dashEndSpeed * dir.normalized;
        while (Time.time - startTime <= unit_attribute.dashEndTime)
        {
            yield return null;
        }
    }
    #endregion
}
