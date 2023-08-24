using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public UnitData playerData;


    void Update()
    {
        HpControl();
    }
    void HpControl()
    {
        if (playerData.hp < 0)
        {
            playerData.hp = 0;
        }
    }
}
