using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public UnitData playerData;

    void Update()
    {
        HpControl(playerData.hp);
    }
    void HpControl(int currentHp)
    {
        if (currentHp > playerData.maxHp)
        {
            currentHp = playerData.maxHp;
        }
        if (currentHp == 0)
        {

        }
        else if (currentHp < 0)
        {
            currentHp = 0;
        }
        playerData.hp = currentHp;
    }
}
