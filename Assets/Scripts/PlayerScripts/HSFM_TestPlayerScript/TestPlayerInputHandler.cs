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
    public bool DoubleJumpInput { get; set; }
    public bool MeleeStopInput { get; private set; }
    #endregion

    public float LastPressedJumpTime { get; private set; }
    public float LastPressedMeleeTime { get; private set; }
    public float LastPressedDashTime { get; private set; }
    public int JumpCount { get; set; }
    public bool IsJumping { get; set; }

    public bool IsJumpCut { get; private set; }


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
            OnJumpInput();
            // Debug.Log("LastPressedJumpTime: " + LastPressedJumpTime);
            SetJumpInput(true);
            SetJumpCutInput(true);
            if (IsJumping && PlayerAbilityManager.CanDoubleJump)
                SetDoubleJumpInput(true);
            // SetJumping(true);
            SetJumpCut(false);
        }
        if (Input.GetButtonUp("Jump"))
        {
            SetJumpInput(false);
            SetJumpCutInput(false);
            SetDoubleJumpInput(false);
            // SetJumping(false);
            SetJumpCut(true);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q is pressed");
            OnMeleeInput();
            MeleeStopInput = false;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            MeleeStopInput = true;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("C is ppressed");
            OnDashInput();
        }

    }

    public void SetJumpInput(bool setting) => JumpInput = setting;
    public void SetJumpCutInput(bool setting) => JumpCutInput = setting;
    public void SetDoubleJumpInput(bool setting) => DoubleJumpInput = setting;
    public void SetJumping(bool setting) => IsJumping = setting;

    public void SetJumpCut(bool setting) => IsJumpCut = setting;


    // public bool JumpInput() => LastPressedJumpTime > 0;
    // public bool MeleeInput() => LastPressedMeleeTime > 0;
    // public bool DashInput() => LastPressedDashTime > 0;
    public void OnJumpInput() => LastPressedJumpTime = attribute.JumpInputBufferTime;
    public void OnMeleeInput() => LastPressedMeleeTime = attribute.MeleeInputBufferTime;
    public void OnDashInput() => LastPressedDashTime = attribute.DashInputBufferTime;

    public void UseJumpInput() => LastPressedJumpTime = 0;
    // public void UseMeleeInput() => LastPressedMeleeTime = 0;
    // public void UseDashInput() => LastPressedDashTime = 0;
}
