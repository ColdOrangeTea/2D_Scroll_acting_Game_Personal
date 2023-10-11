using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    [SerializeField]
    private TestPlayerController player;
    [SerializeField]
    private PlayerInputHandler inputHandler;
    [SerializeField]
    private PlayerPhysicsCheck physicsCheck;
    [SerializeField]
    private PlayerAttribute attribute;

    #region INPUT PARAMETERS
    private float x_input { get; set; }
    private bool isjumping { get; set; }
    private bool isjumpcut { get; set; }
    #endregion
    public void GetPlayerAttribute(object source, UnitAttributeEventArgs args)
    {
        this.player = args.Player;
        this.inputHandler = args.Player.InputHandler;
        this.physicsCheck = args.Player.PhysicsCheck;
        this.attribute = args.Player.Attribute;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    #region MOVE METHOD

    public void GroundMove(float lerpAmount, float xInput, float maxSpeed, float accel, float deccel)
    {
        PlayerPhysicsCheck physicsCheck = this.physicsCheck;

        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = xInput * maxSpeed;

        targetSpeed = Mathf.Lerp(physicsCheck.RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate

        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : deccel;

        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - physicsCheck.RB.velocity.x;

        //Calculate force along x-axis to apply to thr player
        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        physicsCheck.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }

    public void InAirMove(float lerpAmount, float xInput, float maxSpeed, float accel, float deccel, float jumpHangTimeThreshold, float jumpHangAccelerationMult, float jumpHangMaxSpeedMult, bool doConserveMomentum, bool isAnyJumping)
    {
        PlayerPhysicsCheck physicsCheck = this.physicsCheck;

        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = xInput * maxSpeed;

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
    public void Move(float lerpAmount)
    {
        PlayerPhysicsCheck physicsCheck = this.physicsCheck;
        PlayerInputHandler inputHandler = this.inputHandler;
        PlayerAttribute attribute = this.attribute;

        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = inputHandler.XInput * attribute.RunMaxSpeed;
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
        targetSpeed = Mathf.Lerp(physicsCheck.RB.velocity.x, targetSpeed, lerpAmount);


        #region Calculate AccelRate
        float accelRate;


        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.

        if (physicsCheck.LastOnGroundTime > 0) //LastOnGroundTime > 0
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? attribute.RunAccelAmount : attribute.RunDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? attribute.RunAccelAmount * attribute.AccelInAir : attribute.RunDeccelAmount * attribute.DeccelInAir;

        #endregion

        isjumping = player.IsJumping;

        #region Add Bonus Jump Apex Acceleration
        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((isjumping) && Mathf.Abs(physicsCheck.RB.velocity.y) < attribute.JumpHangTimeThreshold)
        {
            accelRate *= attribute.JumpHangAccelerationMult;
            targetSpeed *= attribute.JumpHangMaxSpeedMult;
        }
        #endregion


        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (attribute.DoConserveMomentum && Mathf.Abs(physicsCheck.RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(physicsCheck.RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && physicsCheck.LastOnGroundTime < 0)
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
        float speedDif = targetSpeed - physicsCheck.RB.velocity.x;
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
        physicsCheck.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    public void CrouchSlideMove(float lerpAmount)
    {
        PlayerPhysicsCheck physicsCheck = this.physicsCheck;
        PlayerAttribute attribute = this.attribute;

        //Calculate the direction we want to move in and our desired velocity

        //在斜坡上會改變
        float targetSpeed = 0;

        targetSpeed = Mathf.Lerp(physicsCheck.RB.velocity.x, targetSpeed, lerpAmount);


        #region Calculate AccelRate
        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.

        //if在斜坡上
        if (physicsCheck.LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? attribute.CrouchSlideAccelAmount : attribute.CrouchSlideDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? attribute.CrouchSlideAccelAmount * attribute.AccelOnGround : attribute.CrouchSlideDeccelAmount * attribute.DeccelOnGround;

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
        float speedDif = targetSpeed - physicsCheck.RB.velocity.x;
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
        physicsCheck.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
    public void CrouchIdleMove()
    {
        PlayerPhysicsCheck physicsCheck = this.physicsCheck;
        physicsCheck.RB.velocity = new Vector2(0, physicsCheck.RB.velocity.y);
    }

    #endregion

    #region JUMP METHOD
    public void Jump()
    {
        PlayerPhysicsCheck physicsCheck = this.physicsCheck;
        PlayerAttribute attribute = this.attribute;

        //Ensures we can't call Jump multiple times from one press
        physicsCheck.SetLastOnGroundTimeToZero();


        #region Perform Jump
        //We increase the force applied if we are falling
        //This means we'll always feel like we jump the same amount 
        //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
        float force = attribute.JumpForce;
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

    public void WallJump()
    {
        PlayerAttribute attribute = this.attribute;

        //Ensures we can't call Wall Jump multiple times from one press
        physicsCheck.SetLastOnGroundTimeToZero();
        float lastWallJumpDir = (physicsCheck.LastOnWallRightTime > 0) ? -1 : 1;

        physicsCheck.SetLastOnWallRightTimeToZero();
        physicsCheck.SetLastOnWallLeftTimeToZero();

        #region Perform Wall Jump

        Vector2 force = new Vector2(attribute.WallJumpForce.x, attribute.WallJumpForce.y);
        force.x *= lastWallJumpDir; //apply force in opposite direction of wall

        if (Mathf.Sign(physicsCheck.RB.velocity.x) != Mathf.Sign(force.x))
            force.x -= physicsCheck.RB.velocity.x;

        if (physicsCheck.RB.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
            force.y -= physicsCheck.RB.velocity.y;

        //Unlike in the run we want to use the Impulse mode.
        //The default mode will apply are force instantly ignoring masss

        //CreateJumpDust();

        physicsCheck.RB.AddForce(force, ForceMode2D.Impulse);
        #endregion
    }

    #endregion

    #region WALL SLIDE METHOD
    public void WallSlide()
    {
        PlayerPhysicsCheck physicsCheck = this.physicsCheck;
        PlayerAttribute attribute = this.attribute;
        //Works the same as the Run but only in the y-axis
        //THis seems to work fine, buit maybe you'll find a better way to implement a slide into this system
        //Debug.Log(RB.velocity);
        if (physicsCheck.RB.velocity.y >= attribute.SlideSpeed)
        {
            physicsCheck.RB.velocity = new Vector2(physicsCheck.RB.velocity.x, physicsCheck.RB.velocity.y - 1);
        }
        float speedDif = attribute.SlideSpeed - physicsCheck.RB.velocity.y;
        float movement = Mathf.Abs(speedDif * attribute.SlideAccel);
        //So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
        //The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        physicsCheck.RB.AddForce(movement * Vector2.down);
    }
    public void Grab()
    {
        PlayerPhysicsCheck physicsCheck = this.physicsCheck;
        physicsCheck.RB.velocity = Vector2.zero;
    }
    #endregion


    #region GRAVITY

    private void Gravity()
    {
        PlayerPhysicsCheck physicsCheck = this.physicsCheck;
        PlayerAttribute attribute = this.attribute;

        isjumping = player.IsJumping;
        isjumpcut = player.IsJumpCut;

        if (isjumpcut)
        {
            //Higher gravity if jump button released
            //Debug.Log("cutcut");
            SetGravityScale(attribute.GravityScale * attribute.JumpCutGravityMult);
            physicsCheck.RB.velocity = new Vector2(physicsCheck.RB.velocity.x, Mathf.Max(physicsCheck.RB.velocity.y, -attribute.MaxFallSpeed));
        }
        else if ((isjumping) && Mathf.Abs(physicsCheck.RB.velocity.y) < attribute.JumpHangTimeThreshold)
        {
            SetGravityScale(attribute.GravityScale * attribute.JumpHangGravityMult);
        }
        else if (physicsCheck.RB.velocity.y < 0f && physicsCheck.LastOnGroundTime < 0)
        {
            //Higher gravity if falling
            SetGravityScale(attribute.GravityScale * attribute.FallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            physicsCheck.RB.velocity = new Vector2(physicsCheck.RB.velocity.x, Mathf.Max(physicsCheck.RB.velocity.y, -attribute.MaxFallSpeed));
        }
        else
        {
            //Debug.Log("normal");
            SetGravityScale(attribute.GravityScale);
        }
    }
    public void SetGravityScale(float scale)
    {
        PlayerPhysicsCheck physicsCheck = this.physicsCheck;
        physicsCheck.RB.gravityScale = scale;
    }

    #endregion

    #region OTHER METHODS

    // private void AnimationTrigger() => PlayerStateMachine.CurrentState.AnimationTrigger();
    // private void AnimationFinishTrigger() => PlayerStateMachine.CurrentState.AnimationFinishTrigger();

    #endregion



    #region PLAYER Functions

    public void Punch()
    {
        PlayerPhysicsCheck physicsCheck = this.physicsCheck;

        List<Collider2D> hitEnemies = physicsCheck.CheckHittedUnit();
        foreach (Collider2D Enemy in hitEnemies)
        {
            // Debug.Log(Enemy.name);
            //doDMG
        }
    }

    public void RotateAimPivot()
    {
        PlayerPhysicsCheck physicsCheck = this.physicsCheck;
        PlayerInputHandler inputHandler = this.inputHandler;

        Vector2 dir = inputHandler.RawPointerDirectionInput;

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
        PlayerPhysicsCheck physicsCheck = this.physicsCheck;
        PlayerAttribute attribute = this.attribute;
        //Overall this method of dashing aims to mimic Celeste, if you're looking for
        // a more physics-based approach try a method similar to that used in the jump
        float startTime = Time.time;
        //_isDashAttacking = true;

        SetGravityScale(0);

        //We keep the player's velocity at the dash speed during the "attack" phase (in celeste the first 0.15s)
        while (Time.time - startTime <= attribute.DashAttackTime)
        {
            physicsCheck.RB.velocity = dir.normalized * attribute.DashSpeed;
            //Pauses the loop until the next frame, creating something of a Update loop. 
            //This is a cleaner implementation opposed to multiple timers and this coroutine approach is actually what is used in Celeste :D
            yield return null;
        }
        startTime = Time.time;
        //Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
        SetGravityScale(attribute.GravityScale);
        physicsCheck.RB.velocity = attribute.DashEndSpeed * dir.normalized;
        while (Time.time - startTime <= attribute.DashEndTime)
        {
            yield return null;
        }
    }
    #endregion
}
