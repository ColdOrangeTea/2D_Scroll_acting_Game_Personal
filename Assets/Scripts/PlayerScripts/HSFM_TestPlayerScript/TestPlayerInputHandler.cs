using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class TestPlayerInputHandler : MonoBehaviour
{
    #region  INPUT
    public float XInput { get; private set; }
    public float YInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpCutInput { get; private set; }
    public bool MeleeInput { get; private set; }
    public bool DashInput { get; private set; }
    #endregion

    public float LastPressedJumpTime { get; private set; }
    public float LastPressedMeleeTime { get; private set; }
    public float LastPressedDashTime { get; private set; }
    public int JumpCount { get; set; }
    public bool IsJumping { get; set; }
    public bool IsJumpCut { get; private set; }
    public bool dashUsed;


    [SerializeField] private PlayerAttribute attribute;
    public void GetPlayerAttribute(object source, UnitAttributeEventArgs args)
    {
        this.attribute = args.Player.Attribute;
    }

    // Update is called once per frame
    void Update()
    {
        LastPressedJumpTime -= Time.deltaTime;
        LastPressedMeleeTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;

        XInput = Input.GetAxisRaw("Horizontal");
        YInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            // Debug.Log("LastPressedJumpTime: " + LastPressedJumpTime);
            OnJumpInput();

            SetJumpInput(true);
            SetJumpCutInput(true);

            if (IsJumping && PlayerAbilityManager.CanDoubleJump)
                SetJumpCut(false);
            JumpCount += 1;
        }
        if (Input.GetButtonUp("Jump"))
        {
            SetJumpInput(false);
            SetJumpCutInput(false);
            SetJumpCut(true);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q is pressed");
            OnMeleeInput();
            SetMeleeInput(true);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            SetMeleeInput(false);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("C is ppressed");
            OnDashInput();
            SetDashInput(true);
            dashUsed = true;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            SetDashInput(false);
        }
    }

    public void SetJumpInput(bool setting) => JumpInput = setting;
    public void SetJumpCutInput(bool setting) => JumpCutInput = setting;
    public void SetMeleeInput(bool setting) => MeleeInput = setting;
    public void SetDashInput(bool setting) => DashInput = setting;


    public void SetJumping(bool setting) => IsJumping = setting;
    public void SetJumpCut(bool setting) => IsJumpCut = setting;


    // public bool JumpInput() => LastPressedJumpTime > 0;
    public bool IfMeleeTimeIsOver() => LastPressedMeleeTime <= 0;
    public bool IfDashTimeIsOver() => LastPressedDashTime <= 0;
    public void OnJumpInput() => LastPressedJumpTime = attribute.JumpInputBufferTime;
    public void OnMeleeInput() => LastPressedMeleeTime = attribute.MeleeInputBufferTime;
    public void OnDashInput() => LastPressedDashTime = attribute.DashInputBufferTime;

    // public void UseJumpInput() => LastPressedJumpTime = 0;
    // public void UseMeleeInput() => LastPressedMeleeTime = 0;
    // public void UseDashInput() => LastPressedDashTime = 0;
}
