using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : AbilityState
{
    private bool DashUsed;
    private int DashesLeft = 0;
    private Vector2 _lastDashDir;
    public DashState(PlayerMovement playerMovement, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(playerMovement, stateMachine, unitAttribute, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        DashUsed = false;
        playerMovement.inputHandler.UseDashInput();

    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            if (!DashUsed && DashesLeft > 0)
                //Freeze game for split second. Adds juiciness and a bit of forgiveness over directional input
                playerMovement.Sleep(unitAttribute.dashSleepTime);
            //If not direction pressed, dash forward
            if ((playerMovement.inputHandler.XInput, playerMovement.inputHandler.YInput) != (0, 0))
            {
                _lastDashDir = new Vector2(playerMovement.inputHandler.XInput, playerMovement.inputHandler.YInput).normalized;
            }
            else
                _lastDashDir = new Vector2(playerMovement.FacingDirection, 0);

            playerMovement.GoDash(_lastDashDir);
            DashesLeft--;
            Debug.Log("Dashleft:" + DashesLeft);
            DashUsed = true;
            isAbilityDone = true;
        }
    }
    public bool CheckIfCanDash()
    {
        return DashesLeft > 0;
    }
    public void ResetDashesLeft() => DashesLeft = unitAttribute.dashAmount;
}
