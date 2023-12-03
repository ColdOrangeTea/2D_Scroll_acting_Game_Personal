using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CheatMenu : MonoBehaviour
{
    public static bool CheatInvulnerable = false;
    bool isCheatInvulnerable;
    public Text InvulnerableText;
    public void EnableCheatInvulnerable()
    {
        if (!isCheatInvulnerable)
        {
            Debug.Log("開無敵狀態: " + isCheatInvulnerable);
            isCheatInvulnerable = true;
            CheatInvulnerable = true;
            InvulnerableText.text = "切換無敵模式(目前:開啟)";
        }
        else if (isCheatInvulnerable)
        {
            Debug.Log("關閉無敵狀態: " + isCheatInvulnerable);
            isCheatInvulnerable = false;
            CheatInvulnerable = false;
            InvulnerableText.text = "切換無敵模式(目前:關閉)";

        }


    }
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
