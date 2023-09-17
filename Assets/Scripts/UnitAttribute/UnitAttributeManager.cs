using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttributeManager : MonoBehaviour
{
    // 手動放入Asset
    [SerializeField] protected UnitAttribute unitAttribute;

    // void Update()
    // {
    //     HpControl(unitAttribute.hp);
    // }

    public void HpControl(int currentHp)
    {
        if (currentHp > unitAttribute.maxHp)
        {
            currentHp = unitAttribute.maxHp;
        }
        if (currentHp == 0)
        {

        }
        else if (currentHp < 0)
        {
            currentHp = 0;
        }
        unitAttribute.hp = currentHp;
    }

}
