using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Player : MonoBehaviour
{
    #region STATE VARIABLES
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
    public PlayerPhysicsCheck PhysicsCheck { get; private set; }
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
        PhysicsCheck = GetComponent<PlayerPhysicsCheck>();
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
        PlayerPhysicsCheck physicsCheck = PhysicsCheck;

        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = xInput * maxSpeed;
        // Debug.Log("targetSpeed: " + targetSpeed);
        targetSpeed = Mathf.Lerp(physicsCheck.RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate

        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : deccel;

        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - physicsCheck.RB.velocity.x;
        // Debug.Log("speedDif: " + speedDif);

        //Calculate force along x-axis to apply to thr player
        float movement = speedDif * accelRate;
        // Debug.Log("movement: " + movement);

        //Convert this to a vector and apply to rigidbody
        physicsCheck.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
        // Debug.Log("movement * Vector2.right = physicsCheck.RB.velocity. " + movement * Vector2.right + " = " + physicsCheck.RB.velocity);
    }

    public void InAirMove(float lerpAmount, float xInput, float maxSpeed, float accel, float deccel, float jumpHangTimeThreshold, float jumpHangAccelerationMult, float jumpHangMaxSpeedMult, bool doConserveMomentum, bool isAnyJumping)
    {
        PlayerPhysicsCheck physicsCheck = PhysicsCheck;

        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = xInput * maxSpeed;
        // Debug.Log("targetSpeed: " + targetSpeed);

        targetSpeed = Mathf.Lerp(physicsCheck.RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate

        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : deccel;

        #endregion

        #region Add Bonus Jump Apex Acceleration

        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if (isAnyJumping && Mathf.Abs(physicsCheck.RB.velocity.y) < jumpHangTimeThreshold)
        {
            accelRate *= jumpHangAccelerationMult;
            targetSpeed *= jumpHangMaxSpeedMult;
        }
        #endregion

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (doConserveMomentum && Mathf.Abs(physicsCheck.RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(physicsCheck.RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - physicsCheck.RB.velocity.x;

        //Calculate force along x-axis to apply to thr player
        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        physicsCheck.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }

    #endregion

    #region JUMP METHOD
    public void Jump()
    {
        PlayerPhysicsCheck physicsCheck = PhysicsCheck;

        //Ensures we can't call Jump multiple times from one press
        physicsCheck.SetLastOnGroundTimeToZero();


        #region Perform Jump
        //We increase the force applied if we are falling
        //This means we'll always feel like we jump the same amount 
        //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
        float force = player_attribute.JumpForce;
        /*
		if(LastOnMudTime > 0)
			force = Data.jumpForce / 2f;
		*/

        if (physicsCheck.RB.velocity.y < 0)
            force -= physicsCheck.RB.velocity.y;
        if (physicsCheck.RB.velocity.y > 0)
            physicsCheck.RB.velocity = new Vector2(physicsCheck.RB.velocity.x, 0);

        //CreateJumpDust();

        physicsCheck.RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        #endregion
    }
    #endregion

    #region GRAVITY

    private void Gravity()
    {
        PlayerPhysicsCheck physicsCheck = PhysicsCheck;
        if (PlayerInAirState.IsJumpCut)
        {
            //Higher gravity if jump button released
            //Debug.Log("cutcut");
            SetGravityScale(player_attribute.GravityScale * player_attribute.JumpCutGravityMult);
            physicsCheck.RB.velocity = new Vector2(physicsCheck.RB.velocity.x, Mathf.Max(physicsCheck.RB.velocity.y, -player_attribute.MaxFallSpeed));
        }
        else if ((PlayerInAirState.IsJumping) && Mathf.Abs(physicsCheck.RB.velocity.y) < player_attribute.JumpHangTimeThreshold)
        {
            SetGravityScale(player_attribute.GravityScale * player_attribute.JumpHangGravityMult);
        }
        else if (physicsCheck.RB.velocity.y < 0f && physicsCheck.LastOnGroundTime < 0)
        {
            //Higher gravity if falling
            SetGravityScale(player_attribute.GravityScale * player_attribute.FallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            physicsCheck.RB.velocity = new Vector2(physicsCheck.RB.velocity.x, Mathf.Max(physicsCheck.RB.velocity.y, -player_attribute.MaxFallSpeed));
        }
        else
        {
            //Debug.Log("normal");
            SetGravityScale(player_attribute.GravityScale);
        }
    }
    public void SetGravityScale(float scale)
    {
        PlayerPhysicsCheck physicsCheck = PhysicsCheck;
        physicsCheck.RB.gravityScale = scale;
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
        PlayerPhysicsCheck physicsCheck = PhysicsCheck;

        List<Collider2D> hitted_units = physicsCheck.CheckHittedUnit();
        foreach (Collider2D hitted_unit in hitted_units)
        {
            Debug.Log("擊中: " + hitted_unit.name);
            if (hitted_unit.CompareTag("Thing"))
            {
                Debug.Log("執行 " + hitted_unit.GetComponentInParent<Thing>());
                hitted_unit.GetComponentInParent<Thing>().TriggerThing();
            }
            //doDMG
        }
    }

    public void DoDamage()
    {

    }

    public void TakeDamage()
    {

    }

    public void RotateAimPivot()
    {
        PlayerPhysicsCheck physicsCheck = PhysicsCheck;

        Vector2 dir = InputHandler.RawPointerDirectionInput;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (physicsCheck.RB.transform.localScale.x == -1)
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
        PlayerPhysicsCheck physicsCheck = PhysicsCheck;

        //Overall this method of dashing aims to mimic Celeste, if you're looking for
        // a more physics-based approach try a method similar to that used in the jump
        float startTime = Time.time;
        //_isDashAttacking = true;

        SetGravityScale(0);

        //We keep the player's velocity at the dash speed during the "attack" phase (in celeste the first 0.15s)
        while (Time.time - startTime <= player_attribute.DashAttackTime)
        {
            physicsCheck.RB.velocity = dir.normalized * player_attribute.DashSpeed;
            //Pauses the loop until the next frame, creating something of a Update loop. 
            //This is a cleaner implementation opposed to multiple timers and this coroutine approach is actually what is used in Celeste :D
            yield return null;
        }
        startTime = Time.time;
        //Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
        SetGravityScale(player_attribute.GravityScale);
        physicsCheck.RB.velocity = player_attribute.DashEndSpeed * dir.normalized;
        while (Time.time - startTime <= player_attribute.DashEndTime)
        {
            yield return null;
        }
    }
    #endregion
}
