using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribute : UnitAttributeManager
{
    protected PlayerAttributeText playerAttributeText;

    public override void HpControl(int currentHp)
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

    public override void TakeDamage()
    {
        unitAttribute.hp -= 1;
        HpControl(unitAttribute.hp);
        playerAttributeText.UpdateHP(unitAttribute.hp, unitAttribute.maxHp, unitAttribute.attack);
    }

    public override void Heal()
    {
        unitAttribute.hp += 1;
        HpControl(unitAttribute.hp);
        playerAttributeText.UpdateHP(unitAttribute.hp, unitAttribute.maxHp, unitAttribute.attack);
    }

    public override void InitAttribute()
    {
        // healTest = FindObjectOfType<HealTest>();
        // damageTest = FindObjectOfType<DamageTest>();
        playerAttributeText = FindObjectOfType<PlayerAttributeText>();
        playerAttributeText.UpdateHP(unitAttribute.hp, unitAttribute.maxHp, unitAttribute.attack);
    }

}
