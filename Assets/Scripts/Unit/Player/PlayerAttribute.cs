using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribute : UnitAttributeManager
{
    protected PlayerAttributeText playerAttributeText;

    public override void HpControl(int currentHp)
    {
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        if (currentHp == 0)
        {

        }
        else if (currentHp < 0)
        {
            currentHp = 0;
        }
        hp = currentHp;
    }

    public override void TakeDamage(int Damage)
    {
        hp -= Damage;
        HpControl(hp);
        playerAttributeText.UpdateHP(hp, maxHp, attack);
    }

    public override void Heal()
    {
        hp += 1;
        HpControl(hp);
        playerAttributeText.UpdateHP(hp, maxHp, attack);
    }

    public override void InitAttribute()
    {
        // healTest = FindObjectOfType<HealTest>();
        // damageTest = FindObjectOfType<DamageTest>();
        playerAttributeText = FindObjectOfType<PlayerAttributeText>();

        maxHp = unitAttribute.maxHp;
        hp = unitAttribute.hp;
        maxMp = unitAttribute.maxMp;
        mp = unitAttribute.mp;
        attack = unitAttribute.attack;

        playerAttributeText.UpdateHP(hp, maxHp, attack);
    }

}
