using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribute : UnitAttributeManager
{
    protected HealTest healTest;
    protected DamageTest damageTest;
    protected PlayerAttributeText playerAttributeText;

    void Start()
    {
        healTest = FindObjectOfType<HealTest>();
        damageTest = FindObjectOfType<DamageTest>();
        playerAttributeText = FindObjectOfType<PlayerAttributeText>();
        playerAttributeText.UpdateHP(unitAttribute.hp, unitAttribute.maxHp, unitAttribute.attack);
    }
    public void TakeDamage()
    {
        unitAttribute.hp -= 1;
        playerAttributeText.UpdateHP(unitAttribute.hp, unitAttribute.maxHp, unitAttribute.attack);
    }

    public void Heal()
    {
        unitAttribute.hp += 1;
        playerAttributeText.UpdateHP(unitAttribute.hp, unitAttribute.maxHp, unitAttribute.attack);
    }

}
