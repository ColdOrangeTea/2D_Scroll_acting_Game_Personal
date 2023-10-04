using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region STATE VARIABLES
    public Core Core { get; private set; }
    public EnemyStateMachine EnemyStateMachine { get; private set; }
    public EnemyPhysicCheck EnemyPhysicCheck { get; private set; }
    public IdleState IdleState { get; private set; }
    public MoveState MoveState { get; private set; }
    public JumpState JumpState { get; private set; }
    public MeleeAttackState MeleeAttackState { get; private set; }
    // public DashState DashState { get; private set; }
    public InAirState InAirState { get; private set; }
    public LandState LandState { get; private set; }

    [SerializeField]
    private EnemyAttribute enemy_attribute;

    #endregion

    #region COMPONENTS

    public Animator Anim { get; private set; }
    // public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }



    #endregion

    #region TIMERS
    public float LastOnGroundTime { get; private set; }
    // // public float LastOnWallLeftTime { get; private set; }
    // // public float LastOnWallRightTime { get; private set; }
    // // public float LastOnWallTime { get; private set; }

    #endregion

    #region ANIMATION BOOL NAME

    public const string INAIR = "inAir";

    public const string IDLE = "idle";
    public const string LAND = "land";
    public const string MOVE = "move";

    public const string DASH = "dash";
    public const string JUMP = "jump";
    public const string MELEE = "melee";

    #endregion
    #region OTHER VARIABLES
    // private bool is_facingRight;
    private int facing_direction;
    // private Vector2 current_velocity;

    #endregion
    // #region OTHER VARIABLES
    // public bool IsFacingRight { get; private set; }
    // public int FacingDirection { get; private set; }
    // public Vector2 CurrentVelocity { get; private set; }

    // #endregion
    private void Awake()
    {
        Core = GetComponent<Core>();
        EnemyStateMachine = new EnemyStateMachine();
        IdleState = new IdleState(this, EnemyStateMachine, enemy_attribute, IDLE);
        MoveState = new MoveState(this, EnemyStateMachine, enemy_attribute, MOVE);
        JumpState = new JumpState(this, EnemyStateMachine, enemy_attribute, JUMP);
        MeleeAttackState = new MeleeAttackState(this, EnemyStateMachine, enemy_attribute, MELEE);

        InAirState = new InAirState(this, EnemyStateMachine, enemy_attribute, INAIR);
        LandState = new LandState(this, EnemyStateMachine, enemy_attribute, LAND);
    }

    private void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        EnemyPhysicCheck = GetComponent<EnemyPhysicCheck>();
        RB = GetComponent<Rigidbody2D>();

        EnemyStateMachine.Initialize(IdleState);

        // is_facingRight = EnemyPhysicCheck.IsFacingRight;
        facing_direction = EnemyPhysicCheck.FacingDirection;

        SetGravityScale(enemy_attribute.GravityScale);
    }

    // Update is called once per frame
    private void Update()
    {
        EnemyStateMachine.CurrentState.LogicUpdate();

        LastOnGroundTime -= Time.deltaTime;
        // LastOnWallTime -= Time.deltaTime;
        // LastOnWallRightTime -= Time.deltaTime;
        // LastOnWallLeftTime -= Time.deltaTime;

        Gravity();
    }

    private void FixedUpdate()
    {
        EnemyStateMachine.CurrentState.PhysicsUpdate();
    }

    #region MOVE METHOD

    public void GroundMove(float lerpAmount, float xInput, float maxSpeed, float accel, float deccel)
    {
        Vector3 scale = transform.localScale;
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = scale.x * enemy_attribute.MoveSpeed * maxSpeed;

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
        facing_direction = EnemyPhysicCheck.FacingDirection;
        // 計算我們想要移動的方向和所需的速度
        float targetSpeed = facing_direction * enemy_attribute.MoveMaxSpeed;


        //Debug.Log(targetSpeed);

        // Lerp線性插植 以 lerpAmount 為速率，從 RB.velocity.x 加速到 targetSpeed 的過程
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);


        #region Calculate AccelRate
        float accelRate;


        //根據我們是否加速（包括轉彎）取得加速度值...或減速（停止）如果我們處在空中也會應用乘數。

        if (LastOnGroundTime > 0) //LastOnGroundTime > 0
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? enemy_attribute.MoveAccelAmount : enemy_attribute.MoveDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? enemy_attribute.MoveAccelAmount * enemy_attribute.AccelInAir : enemy_attribute.MoveDeccelAmount * enemy_attribute.DeccelInAir;

        #endregion



        #region Add Bonus Jump Apex Acceleration
        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((InAirState.IsJumping) && Mathf.Abs(RB.velocity.y) < enemy_attribute.JumpHangTimeThreshold)
        {
            accelRate *= enemy_attribute.JumpHangAccelerationMult;
            targetSpeed *= enemy_attribute.jumpHangMaxSpeedMult;
        }
        #endregion


        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (enemy_attribute.DoConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
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

    #endregion

    #region GRAVITY

    private void Gravity()
    {
        if ((InAirState.IsJumping) && Mathf.Abs(RB.velocity.y) < enemy_attribute.JumpHangTimeThreshold)
        {
            SetGravityScale(enemy_attribute.GravityScale * enemy_attribute.JumpHangGravityMult);
        }
        else if (RB.velocity.y < 0f && LastOnGroundTime < 0)
        {
            //Higher gravity if falling
            SetGravityScale(enemy_attribute.GravityScale * enemy_attribute.FallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -enemy_attribute.MaxFallSpeed));
        }
        else
        {
            //Debug.Log("normal");
            SetGravityScale(enemy_attribute.GravityScale);
        }
    }

    #endregion

    #region OTHER METHODS
    public void Turn()
    {
        //stores scale and flips the player along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        Debug.Log("轉身 ");
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


    #region PLAYER Functions
    [Space(5)][Range(0, 50)] public float ProjectileSpeed = 20f;
    public void FireProjectile()
    {
        // GameObject BulletIns = Instantiate(BulletPreFab, FirePoint.position, AimPivot.transform.rotation);
        // BulletIns.GetComponent<Rigidbody2D>().velocity = AimPivot.right * ProjectileSpeed;
    }


    public void MeleeAttack()
    {

        List<Collider2D> hitEnemies = EnemyPhysicCheck.CheckHittedUnit();
        foreach (Collider2D Enemy in hitEnemies)
        {
            Debug.Log(Enemy.name);
            //doDMG
        }
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

    #endregion

}