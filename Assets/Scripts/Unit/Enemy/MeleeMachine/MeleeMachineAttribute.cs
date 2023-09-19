using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMachineAttribute : UnitAttributeManager
{
    public override void HpControl(int currentHp)
    {

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
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
        hp = currentHp;
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.tag == "Attack")
    //        TakeDamage();
    //}

    public override void TakeDamage(int Damage)
    {

        hp -= Damage;
        HpControl(hp);
    }

    public override void Heal()
    {
        hp += 1;
        HpControl(hp);
    }

    public override void InitAttribute()
    {
        maxHp = unitAttribute.maxHp;
        hp = unitAttribute.hp;
        maxMp = unitAttribute.maxMp;
        mp = unitAttribute.mp;
        attack = unitAttribute.attack;
    }

}
