using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPrefab : PortableThing
{
    [SerializeField] private int ThingID = 0;
    [SerializeField] private bool isHeal;
    [SerializeField] private int healAmount = 0;
    [SerializeField] private bool isFunctional;
    [SerializeField] private float thingInvulnerableDuration = 0;

    protected override void Start()
    {
        base.Start();

        if (Attribute.IsHeal)
        {
            // Debug.Log(" IsHeal: " + Attribute.IsHeal);
            isHeal = Attribute.IsHeal;
            healAmount = Attribute.HealAmount;
        }
        else if (Attribute.IsFunctional)
        {
            // Debug.Log(" IsFunctional: " + Attribute.IsFunctional);
            isFunctional = Attribute.IsFunctional;
            if (Attribute.PortableThingID == 1) // 1=潤滑劑
            {
                thingInvulnerableDuration = Attribute.ThingInvulnerableDuration;
            }

        }
        ThingID = Attribute.PortableThingID;
    }
    public bool GetIsHeal()
    {
        return isHeal;
    }
    public bool GetIsFunctional()
    {
        return isFunctional;
    }
    public int GetHealAmount()
    {
        return healAmount;
    }
    public float GetThingInvulnerableDuration()
    {
        return thingInvulnerableDuration;
    }
    public override void PickUp()
    {
        if (ThingID == 10)
        {
            LevelManager.NumOfGears += 1;
            GetComponent<ThingSound>().PlayGear_PickUp();
            Debug.Log("撿取物品: 齒輪+1 ，目前數量:" + LevelManager.NumOfGears);
        }
        if (isFunctional)
        {
            GetComponent<ThingSound>().PlayLubratacation_PickUp();
        }
        else if (isHeal)
        {
            GetComponent<ThingSound>().PlayPotion_PickUp();
        }

    }
    public override void TriggerThing()
    {
        PickUp();
        DestroyThing();
    }
}
