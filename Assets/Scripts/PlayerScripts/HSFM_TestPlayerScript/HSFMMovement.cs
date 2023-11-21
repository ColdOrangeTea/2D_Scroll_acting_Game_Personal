using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSFMMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerHFSMStateManager player;
    [SerializeField]
    private HSFMPlayerInputHandler inputHandler;
    [SerializeField]
    private HSFMPlayerPhysicsCheck physicsCheck;
    [SerializeField]
    private PlayerAttribute attribute;

    #region INPUT PARAMETERS
    // private float x_input { get; set; }
    private bool isjumping { get; set; }
    private bool isjumpcut { get; set; }
    #endregion

    #region TAG NAME
    public const string B_THING = "B_Thing";
    public const string I_THING = "I_Thing";
    public const string P_THING = "P_Thing";
    #endregion
    public void GetPlayerAttribute(object source, UnitAttributeEventArgs args)
    {
        this.player = args.Player;
        this.inputHandler = args.Player.InputHandler;
        this.physicsCheck = args.Player.PhysicsCheck;
        this.attribute = args.Player.Attribute;
    }

    #region MOVE METHOD

    public void GroundMove(float lerpAmount, float xInput, float maxSpeed, float accel, float deccel)
    {
        HSFMPlayerPhysicsCheck physicsCheck = this.physicsCheck;

        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = xInput * maxSpeed;
        // Debug.Log("targetSpeed: " + targetSpeed);
        targetSpeed = Mathf.Lerp(physicsCheck.RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate

        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : deccel;
        // Debug.Log("accelRate: " + accelRate);
        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - physicsCheck.RB.velocity.x;
        // Debug.Log("speedDif: " + speedDif);

        //Calculate force along x-axis to apply to thr player
        float movement = speedDif * accelRate;
        // Debug.Log("movement: " + movement);

        //Convert this to a vector and apply to rigidbody
        physicsCheck.RB.AddForce((movement * Vector2.right) / physicsCheck.RB.gravityScale, ForceMode2D.Force);
        // Debug.Log("(movement * Vector2.right) / physicsCheck.RB.gravityScale, ForceMode2D.Force): " + (movement * Vector2.right) / physicsCheck.RB.gravityScale);

    }

    public void InAirMove(float lerpAmount, float xInput, float maxSpeed, float accel, float deccel, float jumpHangTimeThreshold, float jumpHangAccelerationMult, float jumpHangMaxSpeedMult, bool doConserveMomentum, bool isAnyJumping)
    {
        HSFMPlayerPhysicsCheck physicsCheck = this.physicsCheck;

        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = xInput * maxSpeed;
        targetSpeed = Mathf.Lerp(physicsCheck.RB.velocity.x, targetSpeed, lerpAmount);
        // Debug.Log("targetSpeed: " + targetSpeed);
        SetGravityScale(attribute.GravityScale);
        #region Calculate AccelRate

        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : deccel;
        // Debug.Log("accelRate: " + accelRate);

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
        // Debug.Log("targetSpeed - physicsCheck.RB.velocity.x = speedDif: " + targetSpeed + " - " + physicsCheck.RB.velocity.x + " = " + speedDif);
        // Debug.Log("speedDif * accelRate = movement: " + speedDif + " * " + accelRate + " = " + movement);
        //Convert this to a vector and apply to rigidbody // ForceMode2D.Force 受mass影響(Force /= mass)
        physicsCheck.RB.AddForce((movement * Vector2.right) / physicsCheck.RB.gravityScale, ForceMode2D.Force);
        // Debug.Log("movement * Vector2.right: " + movement * Vector2.right);

    }

    public void Move(float lerpAmount)
    {
        HSFMPlayerPhysicsCheck physicsCheck = this.physicsCheck;
        HSFMPlayerInputHandler inputHandler = this.inputHandler;
        PlayerAttribute attribute = this.attribute;

        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = inputHandler.XInput * attribute.RunMaxSpeed;

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

        // isjumping = player.IsJumping;

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
    #endregion

    #region JUMP METHOD
    public void Jump()
    {
        HSFMPlayerPhysicsCheck physicsCheck = this.physicsCheck;
        PlayerAttribute attribute = this.attribute;

        //Ensures we can't call Jump multiple times from one press
        physicsCheck.SetLastOnGroundTimeToZero();


        #region Perform Jump
        //We increase the force applied if we are falling
        //This means we'll always feel like we jump the same amount 
        //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
        float force = attribute.JumpForce;
        // Debug.Log("force: " + force);
        /*
		if(LastOnMudTime > 0)
			force = Data.jumpForce / 2f;
		*/

        if (physicsCheck.RB.velocity.y < 0)
        {
            force -= physicsCheck.RB.velocity.y;
            // Debug.Log("force: " + force);
        }

        if (physicsCheck.RB.velocity.y > 0)
        {
            physicsCheck.RB.velocity = new Vector2(physicsCheck.RB.velocity.x, 0);
            // Debug.Log("physicsCheck.RB.velocity: " + physicsCheck.RB.velocity);
        }

        physicsCheck.RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        // Debug.Log("Vector2.up * force: " + Vector2.up * force);
        #endregion
    }
    #endregion
    #region GRAVITY

    private void Gravity()
    {
        HSFMPlayerPhysicsCheck physicsCheck = this.physicsCheck;
        PlayerAttribute attribute = this.attribute;

        isjumping = inputHandler.IsJumping;
        isjumpcut = inputHandler.IsJumpCut;

        if (isjumpcut)
        {
            //Higher gravity if jump button released
            // Debug.Log("attribute.GravityScale * attribute.JumpCutGravityMult: " + attribute.GravityScale + "*" + attribute.JumpCutGravityMult + " " + attribute.GravityScale * attribute.JumpCutGravityMult);
            SetGravityScale(attribute.GravityScale * attribute.JumpCutGravityMult);
            physicsCheck.RB.velocity = new Vector2(physicsCheck.RB.velocity.x, Mathf.Max(physicsCheck.RB.velocity.y, -attribute.MaxFallSpeed));
        }
        else if (isjumping && Mathf.Abs(physicsCheck.RB.velocity.y) < attribute.JumpHangTimeThreshold)
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
        HSFMPlayerPhysicsCheck physicsCheck = this.physicsCheck;
        physicsCheck.RB.gravityScale = scale;
    }

    #endregion

    #region OTHER METHODS

    // private void AnimationTrigger() => PlayerStateMachine.CurrentState.AnimationTrigger();
    // private void AnimationFinishTrigger() => PlayerStateMachine.CurrentState.AnimationFinishTrigger();

    #endregion



    #region PLAYER Functions

    #region  PUNCH
    public void Punch()
    {
        HSFMPlayerPhysicsCheck physicsCheck = this.physicsCheck;

        List<Collider2D> hitEnemies = physicsCheck.CheckHittedUnit();
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                Debug.Log("打的敵人是: " + enemy.name);
                enemy.GetComponent<EnemyStatus>().TakeDamage(player.Status.Damage);
                //doDMG
            }

        }
        List<Collider2D> hitted_things = physicsCheck.CheckHittedThing();
        foreach (Collider2D hitted_thing in hitted_things)
        {
            Debug.Log("擊中的物品是: " + hitted_thing.name);
            if (hitted_thing.gameObject.CompareTag(B_THING))
                hitted_thing.GetComponentInParent<Thing>().TriggerThing();
            if (hitted_thing.gameObject.CompareTag(I_THING))
                hitted_thing.GetComponentInParent<Thing>().TriggerThing();

        }
    }
    #endregion

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
    #region  DASH
    public Vector2 SetDashDir()
    {
        // if (!dash_used && dashes_left > 0)
        //     //Freeze game for split second. Adds juiciness and a bit of forgiveness over directional input
        //     player.Sleep(playerAttribute.DashSleepTime);
        //If not direction pressed, dash forward
        Vector2 dir;
        if ((inputHandler.XInput, inputHandler.YInput) != (0, 0))
        {
            dir = new Vector2(inputHandler.XInput, inputHandler.YInput).normalized;
        }
        else
            dir = new Vector2(physicsCheck.FacingDirection, 0);
        return dir;
    }

    public void GoDash(Vector2 dir)
    {
        StartCoroutine(nameof(StartDash), dir);
    }
    private IEnumerator StartDash(Vector2 dir)
    {
        HSFMPlayerPhysicsCheck physicsCheck = this.physicsCheck;
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

    #endregion
}
