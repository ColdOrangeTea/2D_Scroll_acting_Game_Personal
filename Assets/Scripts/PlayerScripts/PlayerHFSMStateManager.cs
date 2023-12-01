using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityHFSM;
public class PlayerHFSMStateManager : MonoBehaviour
{
    [SerializeField]
    private StateMachine fsm;

    #region  COMPONENT
    [SerializeField]
    public Animator animator;
    [SerializeField]
    public HSFMPlayerInputHandler InputHandler; //{ get; private set; }
    [SerializeField]
    public HSFMPlayerPhysicsCheck PhysicsCheck; //{ get; private set; }
    [SerializeField]
    public HSFMMovement Movement;
    [SerializeField]
    public PlayerStatus Status;
    [SerializeField]
    public PlayerAttribute Attribute; //{ get; private set; }
    #endregion

    #region STATE NAME
    const string Idle = "Idle";
    const string Walk = "Walk";
    const string Ground = "Ground";
    const string Fall = "Fall";
    const string InAir = "InAir";
    const string Jump = "Jump";
    const string DoubleJump = "DoubleJump";
    const string Punch = "Punch";
    const string Thunder = "Thunder";
    // const string Dash = "Dash";
    #endregion

    #region  DAMAGE PARAMETER
    [Header("TakeDamage")]
    [SerializeField] private int maxHp;
    [SerializeField] private int hp;
    [SerializeField] private int hurtForce = 10;
    public float InvulnerableDuration;
    private float invulnerableCounter;
    public bool IsInvulnerable;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnPlayerDie;
    #endregion

    void GetComponents()
    {
        animator = GetComponentInChildren<Animator>();
        InputHandler = GetComponent<HSFMPlayerInputHandler>();
        PhysicsCheck = GetComponent<HSFMPlayerPhysicsCheck>();
        Movement = GetComponent<HSFMMovement>();

        SendUnitAttribute sendUnitAttribute = new SendUnitAttribute(); // publisher

        sendUnitAttribute.AttributeDelegated += Movement.GetPlayerAttribute;
        sendUnitAttribute.AttributeDelegated += PhysicsCheck.GetPlayerAttribute;
        sendUnitAttribute.AttributeDelegated += InputHandler.GetPlayerAttribute;
        sendUnitAttribute.SendPlayerAttribute(this);
    }

    void Start()
    {
        GetComponents();
        maxHp = Status.MaxHp;
        hp = Status.Hp;

        fsm = new StateMachine();
        #region GROUND
        var groundFsm = new HybridStateMachine();

        groundFsm.AddState(Idle,
        onEnter: state => { InputHandler.dashUsed = false; InputHandler.JumpCount = 0; animator.SetBool(Idle, true); },
            onLogic: state =>
            {
                // Debug.Log("Idle");
                PhysicsCheck.RB.velocity = new Vector2(0, PhysicsCheck.RB.velocity.y);
                PhysicsCheck.OnGroundCheck(); Movement.SetGravityScale(Attribute.GravityScale); Movement.GroundMove(1, 0, 0, Attribute.RunAccelAmount, Attribute.RunDeccelAmount);
            }, onExit: state => animator.SetBool(Idle, false));

        groundFsm.AddState(Walk, onEnter: state => { animator.SetBool(Walk, true); },
        onLogic: state =>
        {
            // Debug.Log("Walk");
            PhysicsCheck.OnGroundCheck(); Movement.SetGravityScale(Attribute.GravityScale); PhysicsCheck.CheckDirectionToFace_Test();
            Movement.GroundMove(1, InputHandler.XInput, Attribute.RunMaxSpeed, Attribute.RunAccelAmount, Attribute.RunDeccelAmount);
            // Debug.Log(Attribute.GravityScale + " *" + Attribute.FallGravityMult + ": " + Attribute.GravityScale * Attribute.FallGravityMult);
            if (PhysicsCheck.RB.velocity.y < 0.1f && !PhysicsCheck.onGround)
            {
                Movement.SetGravityScale(Attribute.GravityScale * Attribute.FallGravityMult);
                PhysicsCheck.RB.velocity = new Vector2(PhysicsCheck.RB.velocity.x, Mathf.Max(PhysicsCheck.RB.velocity.y, -Attribute.MaxFallSpeed));
            }
        }, onExit: state => animator.SetBool(Walk, false));

        groundFsm.AddTransition(Idle, Walk, transition => Mathf.Abs(InputHandler.XInput) >= Mathf.Epsilon);
        groundFsm.AddTransition(Walk, Idle, transition => InputHandler.XInput == 0 && InputHandler.YInput == 0);

        fsm.AddState(Ground, groundFsm);
        #endregion

        #region  INAIR
        var inAirFsm = new HybridStateMachine();
        fsm.AddTransition(Ground, InAir, transition => PhysicsCheck.onGround && InputHandler.JumpInput == true);
        inAirFsm.AddState(Jump, onEnter: state => { Movement.Jump(); InputHandler.SetJumping(true); animator.SetBool(Jump, true); },
        onLogic =>
        {
            // Debug.Log("Jump");
            PhysicsCheck.OnGroundCheck();
            PhysicsCheck.CheckDirectionToFace_Test();
            Movement.InAirMove(1, InputHandler.XInput, Attribute.RunMaxSpeed, Attribute.RunAccelAmount * Attribute.AccelInAir, Attribute.RunDeccelAmount * Attribute.DeccelInAir, Attribute.JumpHangTimeThreshold, Attribute.JumpHangAccelerationMult, Attribute.JumpHangMaxSpeedMult, Attribute.DoConserveMomentum, PhysicsCheck.RB.velocity.y > 0);
            if (InputHandler.JumpCutInput)
            {
                Movement.SetGravityScale(Attribute.GravityScale * Attribute.JumpCutGravityMult);
                PhysicsCheck.RB.velocity = new Vector2(PhysicsCheck.RB.velocity.x, Mathf.Max(PhysicsCheck.RB.velocity.y, -Attribute.MaxFallSpeed));
            }
        },
        onExit: state => { Movement.SetGravityScale(Attribute.GravityScale); InputHandler.SetJumping(false); animator.SetBool(Jump, false); });

        inAirFsm.AddState(DoubleJump, onEnter: state => { Movement.Jump(); InputHandler.SetJumping(true); animator.SetBool(Jump, true); },
         onLogic =>
        {
            // Debug.Log("DoubleJump");
            PhysicsCheck.OnGroundCheck();
            PhysicsCheck.CheckDirectionToFace_Test();
            Movement.InAirMove(1, InputHandler.XInput, Attribute.RunMaxSpeed, Attribute.RunAccelAmount * Attribute.AccelInAir, Attribute.RunDeccelAmount * Attribute.DeccelInAir, Attribute.JumpHangTimeThreshold, Attribute.JumpHangAccelerationMult, Attribute.JumpHangMaxSpeedMult, Attribute.DoConserveMomentum, PhysicsCheck.RB.velocity.y > 0);
            if (InputHandler.JumpCutInput)
            {
                Movement.SetGravityScale(Attribute.GravityScale * Attribute.JumpCutGravityMult);
                PhysicsCheck.RB.velocity = new Vector2(PhysicsCheck.RB.velocity.x, Mathf.Max(PhysicsCheck.RB.velocity.y, -Attribute.MaxFallSpeed));
            }
        },
        onExit: state => { Movement.SetGravityScale(Attribute.GravityScale); InputHandler.SetJumping(false); animator.SetBool(Jump, false); });
        // inAirFsm.AddTransition(Fall, Jump, t => InputHandler.JumpInput && InputHandler.JumpCount == 0);
        // inAirFsm.AddTransition(Fall, DoubleJump, t => InputHandler.JumpInput && InputHandler.JumpCount == 1);
        inAirFsm.AddTransition(Jump, DoubleJump, t => PlayerAbilityManager.CanDoubleJump && InputHandler.JumpInput && InputHandler.JumpCount == 2);

        fsm.AddState(InAir, inAirFsm);
        // fsm.AddState(Jump, inAirFsm);
        // fsm.AddState(DoubleJump, inAirFsm);
        #endregion

        #region  PUNCH
        fsm.AddState(Punch, onEnter: state => { Movement.Punch(); animator.SetBool(Punch, true); },
        onLogic: state =>
        {
            PhysicsCheck.OnGroundCheck();
            PhysicsCheck.RB.velocity = new Vector2(PhysicsCheck.RB.velocity.x, Mathf.Max(PhysicsCheck.RB.velocity.y, -Attribute.MaxFallSpeed));
        },
        onExit: state => { animator.SetBool(Punch, false); }, canExit: state => InputHandler.IfMeleeTimeIsOver(), needsExitTime: true);

        fsm.AddTransition(Ground, Punch, t => !InputHandler.IfMeleeTimeIsOver());
        fsm.AddTransition(InAir, Punch, t => !InputHandler.IfMeleeTimeIsOver());
        fsm.AddTransition(Punch, InAir, t => InputHandler.IfMeleeTimeIsOver() && !PhysicsCheck.onGround && PhysicsCheck.RB.velocity.y < 0.01f && InputHandler.JumpInput);
        #endregion

        #region THUNDER
        fsm.AddState(Thunder, onEnter: state => { Debug.Log("THUNDER Start"); Movement.Thunder(); animator.SetBool(Thunder, true); },
              onLogic: state =>
              {
                  Debug.Log("THUNDER");
                  PhysicsCheck.OnGroundCheck();
                  PhysicsCheck.RB.velocity = new Vector2(PhysicsCheck.RB.velocity.x, Mathf.Max(PhysicsCheck.RB.velocity.y, -Attribute.MaxFallSpeed));
              },
              onExit: state => { animator.SetBool(Thunder, false); }, canExit: state => InputHandler.IfThunderTimeIsOver(), needsExitTime: true);

        fsm.AddTransition(Ground, Thunder, t => PlayerAbilityManager.CanThunder && !InputHandler.IfThunderTimeIsOver());
        fsm.AddTransition(InAir, Thunder, t => PlayerAbilityManager.CanThunder && !InputHandler.IfThunderTimeIsOver());
        fsm.AddTransition(Thunder, InAir, t => InputHandler.IfThunderTimeIsOver() && !PhysicsCheck.onGround && PhysicsCheck.RB.velocity.y < 0.01f && InputHandler.JumpInput);

        #endregion

        // #region DASH  
        // fsm.AddState(Dash, onEnter: state => { Vector2 lastDashDir = Movement.SetDashDir(); Movement.GoDash(lastDashDir); animator.SetBool(Dash, true); },
        // onLogic: state =>
        // {
        //     Movement.InAirMove(1, InputHandler.XInput, Attribute.RunMaxSpeed, Attribute.RunAccelAmount * Attribute.AccelInAir, Attribute.RunDeccelAmount * Attribute.DeccelInAir, Attribute.JumpHangTimeThreshold, Attribute.JumpHangAccelerationMult, Attribute.JumpHangMaxSpeedMult, Attribute.DoConserveMomentum, PhysicsCheck.RB.velocity.y > 0);
        //     PhysicsCheck.OnGroundCheck(); PhysicsCheck.CheckDirectionToFace_Test(); animator.SetBool(Dash, false);
        // });
        // // canExit: state => InputHandler.IfDashTimeIsOver(), needsExitTime: true);

        // fsm.AddTransitionFromAny(Dash, transition => !InputHandler.IfDashTimeIsOver() && InputHandler.DashInput);
        // fsm.AddTransition(Dash, InAir, t => !PhysicsCheck.onGround && InputHandler.JumpInput);
        // #endregion

        fsm.AddTransitionFromAny(Ground, transition => PhysicsCheck.onGround && PhysicsCheck.RB.velocity.y < 0.01f);

        fsm.SetStartState(Ground);
        fsm.Init();
    }

    private void Update()
    {
        fsm.OnLogic();
        Debug.Log("玩家目前狀態: " + fsm.ActiveStateName + " " + InputHandler.JumpCount);
        if (IsInvulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                IsInvulnerable = false;
            }
        }

    }

    #region HEAL
    public void Heal(int healAmount)
    {
        if (hp + healAmount > maxHp)
        {
            hp = maxHp;
        }
        else
        {
            hp += healAmount;
        }
    }
    #endregion

    #region TAKE DAMAGE
    public void PlayerDie()
    {
        OnPlayerDie?.Invoke();
        this.gameObject.SetActive(false);
    }

    public void TakeDamage(Bullet bullet)
    {
        if (IsInvulnerable) return;

        int damage = bullet.GetColliderDamage();

        if (hp - damage > 0)
        {
            hp -= damage;
            TriggerInvulnerable();
            OnTakeDamage?.Invoke(bullet.transform);
        }
        else
        {
            hp = 0;
            PlayerDie();
        }
    }
    public void TakeColliderDamage(EnemyStatus attackerStatus)
    {
        if (IsInvulnerable) return;

        int damage = attackerStatus.GetColliderDamage();

        if (hp - damage > 0)
        {
            hp -= damage;
            TriggerInvulnerable();
            OnTakeDamage?.Invoke(attackerStatus.transform);
        }
        else
        {
            hp = 0;
            PlayerDie();
        }
    }
    public void PickThingTriggerInvulnerable(float invulnerableDuration)
    {
        Debug.Log("撿物品觸發無敵");
        invulnerableCounter = invulnerableDuration;
        IsInvulnerable = true;

        animator.SetTrigger("PickThingTriggerInvulnerable");
    }
    void TriggerInvulnerable()
    {
        if (!IsInvulnerable)
        {
            Debug.Log("觸發無敵");
            invulnerableCounter = InvulnerableDuration;
            IsInvulnerable = true;
        }
    }
    public void HurtForce(Transform attackPosition)
    {
        float jumpHeight = 5f, gs = PhysicsCheck.RB.gravityScale;
        Vector2 dir = new Vector2((transform.position.x - attackPosition.position.x) * 10, jumpHeight).normalized;

        // PhysicsCheck.RB.velocity = Vector2.zero;

        PhysicsCheck.RB.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        Debug.Log("HurtForce " + dir + " " + dir * hurtForce);
    }
    public void HurtAnimator()
    {
        animator.SetTrigger("Hurt");
        // Debug.Log(" animator.SetTrigger");
    }
    #endregion


}
