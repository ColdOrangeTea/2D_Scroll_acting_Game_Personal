using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatMenu : MonoBehaviour
{
    public void DoubleJumpEnable()
    {
        PlayerAbilityManager.CanDoubleJump = true;
        Debug.Log("二段跳狀態: " + PlayerAbilityManager.CanDoubleJump);
    }
    public void DoubleJumpUnenable()
    {
        PlayerAbilityManager.CanDoubleJump = false;
        Debug.Log("二段跳狀態: " + PlayerAbilityManager.CanDoubleJump);

    }
    public void ThunderEnable()
    {
        PlayerAbilityManager.CanThunder = true;
        Debug.Log("閃電攻擊狀態: " + PlayerAbilityManager.CanThunder);

    }
    public void ThunderUnenable()
    {
        PlayerAbilityManager.CanThunder = false;
        Debug.Log("閃電攻擊狀態: " + PlayerAbilityManager.CanThunder);
    }
}
