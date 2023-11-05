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
    const string ExtractIntel = "ExtractIntel";
    const string Idle = "Idle";
    const string Walk = "Walk";
    const string Jump = "Jump";
    const string DoubleJump = "DoubleJump";
    const string Fall = "Fall";
    const string Ground = "Ground";
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
        var groundFsm = new HybridStateMachine();

        groundFsm.AddState(Idle,
        onEnter: state => { dashes_left = Attribute.DashAmount; },
            onLogic: state =>
            {
                Debug.Log("IdleonLogic");
                PhysicsCheck.OnGroundCheck(); Movement.SetGravityScale(Attribute.GravityScale); Movement.GroundMove(1, 0, 0, Attribute.RunAccelAmount, Attribute.RunDeccelAmount);
            });

        groundFsm.AddState(Walk, // onEnter: state => ,
        onLogic: state =>
        {
            Debug.Log("WalkonLogic");
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

        fsm.AddState(Jump, onEnter: state => { Debug.Log("Enter JumponLogic"); Movement.Jump(); InputHandler.SetJumping(true); },
        onLogic =>
        {
            Debug.Log("JumponLogic");
            PhysicsCheck.OnGroundCheck();
            PhysicsCheck.CheckDirectionToFace_Test();
            Movement.InAirMove(1, InputHandler.XInput, Attribute.RunMaxSpeed, Attribute.RunAccelAmount * Attribute.AccelInAir, Attribute.RunDeccelAmount * Attribute.DeccelInAir, Attribute.JumpHangTimeThreshold, Attribute.JumpHangAccelerationMult, Attribute.JumpHangMaxSpeedMult, Attribute.DoConserveMomentum, PhysicsCheck.RB.velocity.y > 0);
            if (InputHandler.JumpCutInput)
            {

                Movement.SetGravityScale(Attribute.GravityScale * Attribute.JumpCutGravityMult);
                PhysicsCheck.RB.velocity = new Vector2(PhysicsCheck.RB.velocity.x, Mathf.Max(PhysicsCheck.RB.velocity.y, -Attribute.MaxFallSpeed));
            }
        },
        onExit: state => { Debug.Log("Exit JumponLogic"); Movement.SetGravityScale(Attribute.GravityScale); InputHandler.SetJumpInput(false); InputHandler.SetJumping(false); });

        fsm.AddTransition(Ground, Jump, transition => PhysicsCheck.onGround && InputHandler.JumpInput == true);
        // fsm.AddTransitionFromAny(Ground, transition => PhysicsCheck.onGround && PhysicsCheck.RB.velocity.y < 0.01f);


        fsm.AddState(DoubleJump, onEnter: state => { Movement.Jump(); },
        onLogic =>
        {
            Debug.Log("DoubleJumponLogic");
            PhysicsCheck.OnGroundCheck();
            PhysicsCheck.CheckDirectionToFace_Test();
            Movement.InAirMove(1, InputHandler.XInput, Attribute.RunMaxSpeed, Attribute.RunAccelAmount * Attribute.AccelInAir, Attribute.RunDeccelAmount * Attribute.DeccelInAir, Attribute.JumpHangTimeThreshold, Attribute.JumpHangAccelerationMult, Attribute.JumpHangMaxSpeedMult, Attribute.DoConserveMomentum, PhysicsCheck.RB.velocity.y > 0);
            if (InputHandler.JumpCutInput)
            {

                Movement.SetGravityScale(Attribute.GravityScale * Attribute.JumpCutGravityMult);
                PhysicsCheck.RB.velocity = new Vector2(PhysicsCheck.RB.velocity.x, Mathf.Max(PhysicsCheck.RB.velocity.y, -Attribute.MaxFallSpeed));
            }
        },
        onExit: state => { Movement.SetGravityScale(Attribute.GravityScale); InputHandler.IsJumping = false; });

        fsm.AddTransition(Jump, DoubleJump, transition => !PhysicsCheck.onGround && InputHandler.DoubleJumpInput == true);

        fsm.AddTransitionFromAny(Ground, transition => PhysicsCheck.onGround && PhysicsCheck.RB.velocity.y < 0.01f);
        fsm.Init();
    }
    private void Update()
    {
        fsm.OnLogic();
    }
}
