using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    private bool dash_used;
    private int dashes_left = 0;

    // private Vector2 _lastDashDir;
    private Vector2 last_dash_dir;
    public PlayerDashState(Player player, PlayerStateMachine playerStateMachine, PlayerAttribute playerAttribute, string anim_bool_name) : base(player, playerStateMachine, playerAttribute, anim_bool_name)
    {
    }
    public override void Enter()
    {
        base.Enter();
        dash_used = false;
        player.InputHandler.UseDashInput();

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
            if (!dash_used && dashes_left > 0)
                //Freeze game for split second. Adds juiciness and a bit of forgiveness over directional input
                player.Sleep(playerAttribute.DashSleepTime);
            //If not direction pressed, dash forward
            if ((player.InputHandler.XInput, player.InputHandler.YInput) != (0, 0))
            {
                last_dash_dir = new Vector2(player.InputHandler.XInput, player.InputHandler.YInput).normalized;
            }
            else
                last_dash_dir = new Vector2(player.FacingDirection, 0);

            player.GoDash(last_dash_dir);
            dashes_left--;
            // Debug.Log("Dashleft:" + dashes_left);
            dash_used = true;
            isAbilityDone = true;
        }
    }
    public bool CheckIfCanDash()
    {
        return dashes_left > 0;
    }
    public void ResetDashesLeft() => dashes_left = playerAttribute.DashAmount;
}
