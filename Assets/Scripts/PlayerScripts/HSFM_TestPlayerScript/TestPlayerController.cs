using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityHFSM;
public class TestPlayerController : MonoBehaviour
{
    [SerializeField]
    private StateMachine fsm;

    #region  COMPONENT
    [SerializeField]
    public TestPlayerInputHandler InputHandler; //{ get; private set; }
    [SerializeField]
    public TestPlayerPhysicsCheck PhysicsCheck; //{ get; private set; }
    [SerializeField]
    public TestMovement Movement;
    [SerializeField]
    public PlayerAttribute Attribute; //{ get; private set; }
    #endregion

    #region INPUT PARAMETERS
    private int dashes_left = 0;
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
    const string Dash = "Dash";

    #endregion

    void GetComponents()
    {
        InputHandler = GetComponent<TestPlayerInputHandler>();
        PhysicsCheck = GetComponent<TestPlayerPhysicsCheck>();
        Movement = GetComponent<TestMovement>();

        SendUnitAttribute sendUnitAttribute = new SendUnitAttribute(); // publisher

        sendUnitAttribute.AttributeDelegated += Movement.GetPlayerAttribute;
        sendUnitAttribute.AttributeDelegated += PhysicsCheck.GetPlayerAttribute;
        sendUnitAttribute.AttributeDelegated += InputHandler.GetPlayerAttribute;
        sendUnitAttribute.SendPlayerAttribute(this);
    }
    void Start()
    {
        GetComponents();

        fsm = new StateMachine();
        #region GROUND
        var groundFsm = new HybridStateMachine();

        groundFsm.AddState(Idle,
        onEnter: state => { dashes_left = Attribute.DashAmount; InputHandler.dashUsed = false; InputHandler.JumpCount = 0; },
            onLogic: state =>
            {
                Debug.Log("Idle");
                PhysicsCheck.RB.velocity = new Vector2(0, PhysicsCheck.RB.velocity.y);
                PhysicsCheck.OnGroundCheck(); Movement.SetGravityScale(Attribute.GravityScale); Movement.GroundMove(1, 0, 0, Attribute.RunAccelAmount, Attribute.RunDeccelAmount);
            });

        groundFsm.AddState(Walk, // onEnter: state => ,
        onLogic: state =>
        {
            Debug.Log("Walk");
            PhysicsCheck.OnGroundCheck(); Movement.SetGravityScale(Attribute.GravityScale); PhysicsCheck.CheckDirectionToFace_Test();
            Movement.GroundMove(1, InputHandler.XInput, Attribute.RunMaxSpeed, Attribute.RunAccelAmount, Attribute.RunDeccelAmount);
            // Debug.Log(Attribute.GravityScale + " *" + Attribute.FallGravityMult + ": " + Attribute.GravityScale * Attribute.FallGravityMult);
            if (PhysicsCheck.RB.velocity.y < 0.1f && !PhysicsCheck.onGround)
            {
                Movement.SetGravityScale(Attribute.GravityScale * Attribute.FallGravityMult);
                PhysicsCheck.RB.velocity = new Vector2(PhysicsCheck.RB.velocity.x, Mathf.Max(PhysicsCheck.RB.velocity.y, -Attribute.MaxFallSpeed));
            }
        });

        groundFsm.AddTransition(Idle, Walk, transition => Mathf.Abs(InputHandler.XInput) >= Mathf.Epsilon);
        groundFsm.AddTransition(Walk, Idle, transition => InputHandler.XInput == 0 && InputHandler.YInput == 0);

        fsm.AddState(Ground, groundFsm);
        fsm.SetStartState(Ground);
        #endregion

        #region  INAIR
        var inAirFsm = new HybridStateMachine();
        fsm.AddTransition(Ground, InAir, transition => PhysicsCheck.onGround && InputHandler.JumpInput == true);

        // inAirFsm.AddState(Fall, onEnter: state => { },
        //  onLogic =>
        // {
        //     PhysicsCheck.OnGroundCheck();
        //     PhysicsCheck.CheckDirectionToFace_Test();
        //     Movement.InAirMove(1, InputHandler.XInput, Attribute.RunMaxSpeed, Attribute.RunAccelAmount * Attribute.AccelInAir, Attribute.RunDeccelAmount * Attribute.DeccelInAir, Attribute.JumpHangTimeThreshold, Attribute.JumpHangAccelerationMult, Attribute.JumpHangMaxSpeedMult, Attribute.DoConserveMomentum, PhysicsCheck.RB.velocity.y > 0);
        // },
        // onExit: state => { Movement.SetGravityScale(Attribute.GravityScale); });

        inAirFsm.AddState(Jump, onEnter: state => { Movement.Jump(); InputHandler.SetJumping(true); },
        onLogic =>
        {
            Debug.Log("Jump");
            PhysicsCheck.OnGroundCheck();
            PhysicsCheck.CheckDirectionToFace_Test();
            Movement.InAirMove(1, InputHandler.XInput, Attribute.RunMaxSpeed, Attribute.RunAccelAmount * Attribute.AccelInAir, Attribute.RunDeccelAmount * Attribute.DeccelInAir, Attribute.JumpHangTimeThreshold, Attribute.JumpHangAccelerationMult, Attribute.JumpHangMaxSpeedMult, Attribute.DoConserveMomentum, PhysicsCheck.RB.velocity.y > 0);
            if (InputHandler.JumpCutInput)
            {
                Movement.SetGravityScale(Attribute.GravityScale * Attribute.JumpCutGravityMult);
                PhysicsCheck.RB.velocity = new Vector2(PhysicsCheck.RB.velocity.x, Mathf.Max(PhysicsCheck.RB.velocity.y, -Attribute.MaxFallSpeed));
            }
        },
        onExit: state => { Movement.SetGravityScale(Attribute.GravityScale); InputHandler.SetJumping(false); });

        inAirFsm.AddState(DoubleJump, onEnter: state => { Movement.Jump(); InputHandler.SetJumping(true); },
         onLogic =>
        {
            Debug.Log("DoubleJump");
            PhysicsCheck.OnGroundCheck();
            PhysicsCheck.CheckDirectionToFace_Test();
            Movement.InAirMove(1, InputHandler.XInput, Attribute.RunMaxSpeed, Attribute.RunAccelAmount * Attribute.AccelInAir, Attribute.RunDeccelAmount * Attribute.DeccelInAir, Attribute.JumpHangTimeThreshold, Attribute.JumpHangAccelerationMult, Attribute.JumpHangMaxSpeedMult, Attribute.DoConserveMomentum, PhysicsCheck.RB.velocity.y > 0);
            if (InputHandler.JumpCutInput)
            {
                Movement.SetGravityScale(Attribute.GravityScale * Attribute.JumpCutGravityMult);
                PhysicsCheck.RB.velocity = new Vector2(PhysicsCheck.RB.velocity.x, Mathf.Max(PhysicsCheck.RB.velocity.y, -Attribute.MaxFallSpeed));
            }
        },
        onExit: state => { Movement.SetGravityScale(Attribute.GravityScale); InputHandler.SetJumping(false); });
        // inAirFsm.AddTransition(Fall, Jump, t => InputHandler.JumpInput && InputHandler.JumpCount == 0);
        // inAirFsm.AddTransition(Fall, DoubleJump, t => InputHandler.JumpInput && InputHandler.JumpCount == 1);
        inAirFsm.AddTransition(Jump, DoubleJump, t => InputHandler.JumpInput && InputHandler.JumpCount == 2);

        fsm.AddState(InAir, inAirFsm);
        // fsm.AddState(Jump, inAirFsm);
        // fsm.AddState(DoubleJump, inAirFsm);
        #endregion

        #region  PUNCH
        fsm.AddState(Punch, onEnter: state => { Movement.Punch(); },
        onLogic: state => { PhysicsCheck.OnGroundCheck(); PhysicsCheck.CheckDirectionToFace_Test(); },
        onExit: state => { }, canExit: state => InputHandler.IfMeleeTimeIsOver(), needsExitTime: true);

        fsm.AddTransition(Ground, Punch, t => !InputHandler.IfMeleeTimeIsOver());
        fsm.AddTransition(InAir, Punch, t => !InputHandler.IfMeleeTimeIsOver());
        fsm.AddTransition(Punch, InAir, t => InputHandler.IfMeleeTimeIsOver() && !PhysicsCheck.onGround && PhysicsCheck.RB.velocity.y < 0.01f && InputHandler.JumpInput);
        #endregion

        fsm.AddState(Dash, onEnter: state => { Vector2 lastDashDir = SetDashDir(); Movement.GoDash(lastDashDir); },
        onLogic: state => { PhysicsCheck.OnGroundCheck(); PhysicsCheck.CheckDirectionToFace_Test(); },
        canExit: state => InputHandler.IfDashTimeIsOver(), needsExitTime: true);

        #region DASH
        fsm.AddTransitionFromAny(Dash, transition => !InputHandler.IfDashTimeIsOver() && InputHandler.DashInput);
        fsm.AddTransition(Dash, InAir, t => InputHandler.IfDashTimeIsOver() && !PhysicsCheck.onGround && InputHandler.JumpInput);
        #endregion

        fsm.AddTransitionFromAny(Ground, transition => PhysicsCheck.onGround && PhysicsCheck.RB.velocity.y < 0.01f);
        fsm.Init();
    }
    private void Update()
    {
        fsm.OnLogic();
        Debug.Log("玩家目前狀態: " + fsm.ActiveStateName + " " + InputHandler.JumpCount);
    }

    Vector2 SetDashDir()
    {
        // if (!dash_used && dashes_left > 0)
        //     //Freeze game for split second. Adds juiciness and a bit of forgiveness over directional input
        //     player.Sleep(playerAttribute.DashSleepTime);
        //If not direction pressed, dash forward
        Vector2 dir;
        if ((InputHandler.XInput, InputHandler.YInput) != (0, 0))
        {
            dir = new Vector2(InputHandler.XInput, InputHandler.YInput).normalized;
        }
        else
            dir = new Vector2(PhysicsCheck.FacingDirection, 0);
        return dir;
    }
}
