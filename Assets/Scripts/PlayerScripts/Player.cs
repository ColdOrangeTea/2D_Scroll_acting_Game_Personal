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
    public PlayerPunchState PlayerPunchState { get; private set; }
    public PlayerDashState PlayerDashState { get; private set; }
    public PlayerInAirState PlayerInAirState { get; private set; }
    public PlayerLandState PlayerLandState { get; private set; }

    [SerializeField]
    private PlayerAttribute player_attribute;
    #endregion

    #region COMPONENTS

    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerPhysicsCheck PlayerPhysicCheck { get; private set; }
    #endregion

    #region ANIMATION BOOL NAME

    public const string INAIR = "inAir";

    public const string IDLE = "idle";
    public const string LAND = "land";
    public const string MOVE = "move";

    public const string DASH = "dash";
    public const string JUMP = "jump";
    public const string PUNCH = "punch";

    #endregion

    #region UNITY CALLBACK FUNCTIONS

    private void Awake()
    {
        Core = GetComponentInChildren<Core>();
        PlayerStateMachine = new PlayerStateMachine();

        PlayerInAirState = new PlayerInAirState(this, PlayerStateMachine, player_attribute, INAIR);

        PlayerIdleState = new PlayerIdleState(this, PlayerStateMachine, player_attribute, IDLE);
        PlayerLandState = new PlayerLandState(this, PlayerStateMachine, player_attribute, LAND);
        PlayerMoveState = new PlayerMoveState(this, PlayerStateMachine, player_attribute, MOVE);

        PlayerDashState = new PlayerDashState(this, PlayerStateMachine, player_attribute, DASH);
        PlayerJumpState = new PlayerJumpState(this, PlayerStateMachine, player_attribute, JUMP);
        PlayerPunchState = new PlayerPunchState(this, PlayerStateMachine, player_attribute, PUNCH);
    }

    private void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        PlayerPhysicCheck = GetComponent<PlayerPhysicsCheck>();
        PlayerStateMachine.Initialize(PlayerIdleState);

        SetGravityScale(player_attribute.GravityScale);
    }

    private void Update()
    {
        PlayerStateMachine.CurrentState.LogicUpdate();
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

        targetSpeed = Mathf.Lerp(PlayerPhysicCheck.RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate

        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : deccel;

        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - PlayerPhysicCheck.RB.velocity.x;

        //Calculate force along x-axis to apply to thr player
        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        PlayerPhysicCheck.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }

    public void InAirMove(float lerpAmount, float xInput, float maxSpeed, float accel, float deccel, float jumpHangTimeThreshold, float jumpHangAccelerationMult, float jumpHangMaxSpeedMult, bool doConserveMomentum, bool isAnyJumping)
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = xInput * maxSpeed;

        targetSpeed = Mathf.Lerp(PlayerPhysicCheck.RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate

        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : deccel;

        #endregion

        #region Add Bonus Jump Apex Acceleration

        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if (isAnyJumping && Mathf.Abs(PlayerPhysicCheck.RB.velocity.y) < jumpHangTimeThreshold)
        {
            accelRate *= jumpHangAccelerationMult;
            targetSpeed *= jumpHangMaxSpeedMult;
        }
        #endregion

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (doConserveMomentum && Mathf.Abs(PlayerPhysicCheck.RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(PlayerPhysicCheck.RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - PlayerPhysicCheck.RB.velocity.x;

        //Calculate force along x-axis to apply to thr player
        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        PlayerPhysicCheck.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }
    public void Move(float lerpAmount)
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = InputHandler.XInput * player_attribute.RunMaxSpeed;
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
        targetSpeed = Mathf.Lerp(PlayerPhysicCheck.RB.velocity.x, targetSpeed, lerpAmount);


        #region Calculate AccelRate
        float accelRate;


        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.

        if (PlayerPhysicCheck.LastOnGroundTime > 0) //LastOnGroundTime > 0
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? player_attribute.RunAccelAmount : player_attribute.RunDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? player_attribute.RunAccelAmount * player_attribute.AccelInAir : player_attribute.RunDeccelAmount * player_attribute.DeccelInAir;

        #endregion



        #region Add Bonus Jump Apex Acceleration
        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((PlayerInAirState.IsJumping) && Mathf.Abs(PlayerPhysicCheck.RB.velocity.y) < player_attribute.JumpHangTimeThreshold)
        {
            accelRate *= player_attribute.JumpHangAccelerationMult;
            targetSpeed *= player_attribute.JumpHangMaxSpeedMult;
        }
        #endregion


        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (player_attribute.DoConserveMomentum && Mathf.Abs(PlayerPhysicCheck.RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(PlayerPhysicCheck.RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && PlayerPhysicCheck.LastOnGroundTime < 0)
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
        float speedDif = targetSpeed - PlayerPhysicCheck.RB.velocity.x;
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
        PlayerPhysicCheck.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    public void CrouchSlideMove(float lerpAmount)
    {
        //Calculate the direction we want to move in and our desired velocity

        //在斜坡上會改變
        float targetSpeed = 0;

        targetSpeed = Mathf.Lerp(PlayerPhysicCheck.RB.velocity.x, targetSpeed, lerpAmount);


        #region Calculate AccelRate
        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.

        //if在斜坡上
        if (PlayerPhysicCheck.LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? player_attribute.CrouchSlideAccelAmount : player_attribute.CrouchSlideDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? player_attribute.CrouchSlideAccelAmount * player_attribute.AccelOnGround : player_attribute.CrouchSlideDeccelAmount * player_attribute.DeccelOnGround;

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
        float speedDif = targetSpeed - PlayerPhysicCheck.RB.velocity.x;
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
        PlayerPhysicCheck.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
    public void CrouchIdleMove()
    {
        PlayerPhysicCheck.RB.velocity = new Vector2(0, PlayerPhysicCheck.RB.velocity.y);
    }

    #endregion

    #region JUMP METHOD
    public void Jump()
    {
        //Ensures we can't call Jump multiple times from one press
        PlayerPhysicCheck.SetLastOnGroundTimeToZero();


        #region Perform Jump
        //We increase the force applied if we are falling
        //This means we'll always feel like we jump the same amount 
        //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
        float force = player_attribute.JumpForce;
        /*
		if(LastOnMudTime > 0)
			force = Data.jumpForce / 2f;
		*/

        if (PlayerPhysicCheck.RB.velocity.y < 0)
            force -= PlayerPhysicCheck.RB.velocity.y;
        if (PlayerPhysicCheck.RB.velocity.y > 0)
            PlayerPhysicCheck.RB.velocity = new Vector2(PlayerPhysicCheck.RB.velocity.x, 0);

        //CreateJumpDust();

        PlayerPhysicCheck.RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        #endregion
    }

    public void WallJump()
    {
        //Ensures we can't call Wall Jump multiple times from one press
        PlayerPhysicCheck.SetLastOnGroundTimeToZero();
        float lastWallJumpDir = (PlayerPhysicCheck.LastOnWallRightTime > 0) ? -1 : 1;

        PlayerPhysicCheck.SetLastOnWallRightTimeToZero();
        PlayerPhysicCheck.SetLastOnWallLeftTimeToZero();

        #region Perform Wall Jump

        Vector2 force = new Vector2(player_attribute.WallJumpForce.x, player_attribute.WallJumpForce.y);
        force.x *= lastWallJumpDir; //apply force in opposite direction of wall

        if (Mathf.Sign(PlayerPhysicCheck.RB.velocity.x) != Mathf.Sign(force.x))
            force.x -= PlayerPhysicCheck.RB.velocity.x;

        if (PlayerPhysicCheck.RB.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
            force.y -= PlayerPhysicCheck.RB.velocity.y;

        //Unlike in the run we want to use the Impulse mode.
        //The default mode will apply are force instantly ignoring masss

        //CreateJumpDust();

        PlayerPhysicCheck.RB.AddForce(force, ForceMode2D.Impulse);
        #endregion
    }

    #endregion

    #region WALL SLIDE METHOD
    public void WallSlide()
    {
        //Works the same as the Run but only in the y-axis
        //THis seems to work fine, buit maybe you'll find a better way to implement a slide into this system
        //Debug.Log(RB.velocity);
        if (PlayerPhysicCheck.RB.velocity.y >= player_attribute.SlideSpeed)
        {
            PlayerPhysicCheck.RB.velocity = new Vector2(PlayerPhysicCheck.RB.velocity.x, PlayerPhysicCheck.RB.velocity.y - 1);
        }
        float speedDif = player_attribute.SlideSpeed - PlayerPhysicCheck.RB.velocity.y;
        float movement = Mathf.Abs(speedDif * player_attribute.SlideAccel);
        //So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
        //The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        PlayerPhysicCheck.RB.AddForce(movement * Vector2.down);
    }
    public void Grab()
    {
        PlayerPhysicCheck.RB.velocity = Vector2.zero;
    }
    #endregion


    #region GRAVITY

    private void Gravity()
    {

        if (PlayerInAirState.IsJumpCut)
        {
            //Higher gravity if jump button released
            //Debug.Log("cutcut");
            SetGravityScale(player_attribute.GravityScale * player_attribute.JumpCutGravityMult);
            PlayerPhysicCheck.RB.velocity = new Vector2(PlayerPhysicCheck.RB.velocity.x, Mathf.Max(PlayerPhysicCheck.RB.velocity.y, -player_attribute.MaxFallSpeed));
        }
        else if ((PlayerInAirState.IsJumping) && Mathf.Abs(PlayerPhysicCheck.RB.velocity.y) < player_attribute.JumpHangTimeThreshold)
        {
            SetGravityScale(player_attribute.GravityScale * player_attribute.JumpHangGravityMult);
        }
        else if (PlayerPhysicCheck.RB.velocity.y < 0f && PlayerPhysicCheck.LastOnGroundTime < 0)
        {
            //Higher gravity if falling
            SetGravityScale(player_attribute.GravityScale * player_attribute.FallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            PlayerPhysicCheck.RB.velocity = new Vector2(PlayerPhysicCheck.RB.velocity.x, Mathf.Max(PlayerPhysicCheck.RB.velocity.y, -player_attribute.MaxFallSpeed));
        }
        else
        {
            //Debug.Log("normal");
            SetGravityScale(player_attribute.GravityScale);
        }
    }
    public void SetGravityScale(float scale)
    {
        PlayerPhysicCheck.RB.gravityScale = scale;
    }

    #endregion

    #region OTHER METHODS

    private void AnimationTrigger() => PlayerStateMachine.CurrentState.AnimationTrigger();
    private void AnimationFinishTrigger() => PlayerStateMachine.CurrentState.AnimationFinishTrigger();

    #endregion



    #region PLAYER Functions
    [Space(5)][Range(0, 50)] public float ProjectileSpeed = 20f;
    public void FireProjectile()
    {
        // GameObject BulletIns = Instantiate(BulletPreFab, FirePoint.position, AimPivot.transform.rotation);
        // BulletIns.GetComponent<Rigidbody2D>().velocity = AimPivot.right * ProjectileSpeed;
    }

    public void Punch()
    {
        List<Collider2D> hitEnemies = PlayerPhysicCheck.CheckHittedUnit();
        foreach (Collider2D Enemy in hitEnemies)
        {
            // Debug.Log(Enemy.name);
            //doDMG
        }
    }

    public void RotateAimPivot()
    {
        Vector2 dir = InputHandler.RawPointerDirectionInput;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (PlayerPhysicCheck.RB.transform.localScale.x == -1)
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
        while (Time.time - startTime <= player_attribute.DashAttackTime)
        {
            PlayerPhysicCheck.RB.velocity = dir.normalized * player_attribute.DashSpeed;
            //Pauses the loop until the next frame, creating something of a Update loop. 
            //This is a cleaner implementation opposed to multiple timers and this coroutine approach is actually what is used in Celeste :D
            yield return null;
        }
        startTime = Time.time;
        //Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
        SetGravityScale(player_attribute.GravityScale);
        PlayerPhysicCheck.RB.velocity = player_attribute.DashEndSpeed * dir.normalized;
        while (Time.time - startTime <= player_attribute.DashEndTime)
        {
            yield return null;
        }
    }
    #endregion
}
