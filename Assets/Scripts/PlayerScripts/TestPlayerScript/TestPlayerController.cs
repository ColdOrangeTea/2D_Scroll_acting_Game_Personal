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
    public PlayerInputHandler InputHandler; //{ get; private set; }
    [SerializeField]
    public PlayerPhysicsCheck PhysicsCheck; //{ get; private set; }
    [SerializeField]
    public TestMovement Movement;
    [SerializeField]
    public PlayerAttribute Attribute; //{ get; private set; }
    #endregion

    #region INPUT PARAMETERS
    public float XInput { get; set; }
    public float YInput { get; set; }
    private bool jump_cut_input;
    public bool IsJumping { get; set; }
    public bool IsJumpCut { get; private set; }

    #endregion

    #region STATE NAME
    const string ExtractIntel = "ExtractIntel";
    const string Idle = "Idle";
    const string Move = "Move";
    const string Jump = "Jump";
    const string InAir = "InAir";

    #endregion

    public float playerScanningRange = 4f;
    public float ownScanningRange = 6f;
    void ThereIsNoInput()
    {
        XInput = InputHandler.XInput;
        YInput = InputHandler.YInput;
    }
    public bool CanJumpCut() => IsJumping && PhysicsCheck.CurrentVelocity.y > 0;
    public void SetJumping(bool setting) => IsJumping = setting;
    public void SetJumpCut(bool setting) => IsJumpCut = setting;

    private void CheckJumping()
    {
        if (IsJumping && PhysicsCheck.CurrentVelocity.y < 0)
            IsJumping = false;
    }
    private void CheckJumpCut()
    {
        if (jump_cut_input && CanJumpCut())
        {
            IsJumpCut = true;
        }
    }
    void Start()
    {
        InputHandler = GetComponent<PlayerInputHandler>();
        PhysicsCheck = GetComponent<PlayerPhysicsCheck>();
        Movement = GetComponent<TestMovement>();

        SendUnitAttribute sendUnitAttribute = new SendUnitAttribute(); // publisher

        sendUnitAttribute.AttributeDelegated += Movement.GetPlayerAttribute;
        sendUnitAttribute.SendPlayerAttribute(this);

        fsm = new StateMachine();

        fsm.AddState(ExtractIntel);  // Empty state without any logic.

        fsm.AddState(Idle, new State(onLogic: state => Movement.GroundMove(1, 0, 0, Attribute.RunAccelAmount, Attribute.RunDeccelAmount)));

        fsm.AddState(Move, new State(onLogic: state => Movement.GroundMove(1, 0, 0, Attribute.RunAccelAmount, Attribute.RunDeccelAmount)));

        fsm.AddState(Jump, new State(onLogic: state => SetJumping(true)));

        fsm.AddState(InAir, new State(onLogic: state => Movement.InAirMove(
            1, XInput, Attribute.RunMaxSpeed, Attribute.RunAccelAmount * Attribute.AccelInAir,
            Attribute.RunDeccelAmount * Attribute.DeccelInAir, Attribute.JumpHangTimeThreshold,
            Attribute.JumpHangAccelerationMult, Attribute.JumpHangMaxSpeedMult, Attribute.DoConserveMomentum, IsJumping)));

        // fsm.AddTransition(new Transition(ExtractIntel, Idle, transition =>XInput==null))
    }
}
