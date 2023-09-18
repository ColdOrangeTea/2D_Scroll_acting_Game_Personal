using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMachineAttribute : UnitAttributeManager
{
    public override void HpControl(int currentHp)
    {
        if (currentHp > unitAttribute.maxHp)
        {
            currentHp = unitAttribute.maxHp;
        }
        if (currentHp == 0)
        {
            MeleeMachineStateMachineManager meleeMachineStateMachineManager = GetComponent<MeleeMachineStateMachineManager>();
            meleeMachineStateMachineManager.SwitchStatus(UnitStateMachineManager.dead);
        }
        else if (currentHp < 0)
        {
            currentHp = 0;
        }
        unitAttribute.hp = currentHp;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Attack")
            TakeDamage();
    }

    public override void TakeDamage()
    {
        unitAttribute.hp -= 1;
        HpControl(unitAttribute.hp);
    }

    public override void Heal()
    {
        unitAttribute.hp += 1;
        HpControl(unitAttribute.hp);
    }

    public override void InitAttribute()
    {

    }

}
