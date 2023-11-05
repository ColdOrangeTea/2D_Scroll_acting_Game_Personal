using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    idle,
    walk,
    fall,
    attack,
    chase,
    dead,
}

public class NewEnemy : MonoBehaviour
{
    [SerializeField] public EnemyAttribute enemyAttribute;

    [SerializeField] protected string curState;

    #region COMPONENTS
    public Animator Anim { get; private set; }
    public NewEnemyPhysicsCheck NewEnemyPhysicsCheck { get; private set; }
    #endregion

    #region UNITY CALLBACK FUNCTIONS
    public virtual void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
        NewEnemyPhysicsCheck = GetComponent<NewEnemyPhysicsCheck>();
    }
    public virtual void Start()
    {
        DoChecks();
    }
    public virtual void Update()
    {
        LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        PhysicsUpdate();
    }
    #endregion

    public virtual void DoChecks()
    {

    }
    public virtual void LogicUpdate()
    {

    }
    public virtual void PhysicsUpdate()
    {
        Gravity();
        DoChecks();
    }


    #region MOVE METHOD

    public virtual void GroundMove(float lerpAmount, float xInput, float maxSpeed, float accel, float deccel)
    {
        Vector3 scale = transform.localScale;
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = scale.x * enemyAttribute.MoveSpeed * maxSpeed;

        targetSpeed = Mathf.Lerp(NewEnemyPhysicsCheck.RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate

        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : deccel;

        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - NewEnemyPhysicsCheck.RB.velocity.x;

        //Calculate force along x-axis to apply to thr player
        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        NewEnemyPhysicsCheck.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    public virtual void InAirMove(float lerpAmount, float xInput, float maxSpeed, float accel, float deccel, float jumpHangTimeThreshold, float jumpHangAccelerationMult, float jumpHangMaxSpeedMult, bool doConserveMomentum, bool isAnyJumping)
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = xInput * maxSpeed;

        targetSpeed = Mathf.Lerp(NewEnemyPhysicsCheck.RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate

        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : deccel;

        #endregion

        #region Add Bonus Jump Apex Acceleration

        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if (isAnyJumping && Mathf.Abs(NewEnemyPhysicsCheck.RB.velocity.y) < jumpHangTimeThreshold)
        {
            accelRate *= jumpHangAccelerationMult;
            targetSpeed *= jumpHangMaxSpeedMult;
        }
        #endregion

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (doConserveMomentum && Mathf.Abs(NewEnemyPhysicsCheck.RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(NewEnemyPhysicsCheck.RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - NewEnemyPhysicsCheck.RB.velocity.x;

        //Calculate force along x-axis to apply to thr player
        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        NewEnemyPhysicsCheck.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }
    public virtual void Move(float lerpAmount)
    {
    }

    #endregion

    #region GRAVITY

    protected virtual void Gravity()
    {
        if ((curState == State.fall.ToString()))
        {
            SetGravityScale(enemyAttribute.GravityScale * enemyAttribute.JumpHangGravityMult);
        }
        // if (NewEnemyPhysicsCheck.RB.velocity.y < 0f && NewEnemyPhysicsCheck.LastOnGroundTime < 0)
        // {
        //     //Higher gravity if falling
        //     SetGravityScale(enemyAttribute.GravityScale * enemyAttribute.FallGravityMult);
        //     //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
        //     NewEnemyPhysicsCheck.RB.velocity = new Vector2(NewEnemyPhysicsCheck.RB.velocity.x, Mathf.Max(NewEnemyPhysicsCheck.RB.velocity.y, -enemyAttribute.MaxFallSpeed));
        // }
        else
        {
            //Debug.Log("normal");
            SetGravityScale(enemyAttribute.GravityScale);
        }
    }

    public virtual void SetGravityScale(float scale)
    {
        NewEnemyPhysicsCheck.RB.gravityScale = scale;
    }

    #endregion

    #region OTHER METHODS
    public virtual void Turn()
    {
        //stores scale and flips the player along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        Debug.Log("轉身 ");
    }

    #endregion


    #region ENEMY Functions
    [Space(5)][Range(0, 50)] public float ProjectileSpeed = 20f;
    public virtual void FireProjectile()
    {
        // GameObject BulletIns = Instantiate(BulletPreFab, FirePoint.position, AimPivot.transform.rotation);
        // BulletIns.GetComponent<Rigidbody2D>().velocity = AimPivot.right * ProjectileSpeed;
    }


    public virtual void MeleeAttack()
    {

        List<Collider2D> hitEnemies = NewEnemyPhysicsCheck.CheckHittedUnit();
        foreach (Collider2D Enemy in hitEnemies)
        {
            // Debug.Log(Enemy.name);
            //doDMG
        }
    }

    public virtual void Sleep(float duration)
    {
        //Method used so we don't need to call StartCoroutine everywhere
        //nameof() notation means we don't need to input a string directly.
        //Removes chance of spelling mistakes and will improve error messages if any
        StartCoroutine(nameof(PerformSleep), duration);
    }
    public virtual IEnumerator PerformSleep(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration); //Must be Realtime since timeScale with be 0 
        Time.timeScale = 1;
    }

    #endregion

}
