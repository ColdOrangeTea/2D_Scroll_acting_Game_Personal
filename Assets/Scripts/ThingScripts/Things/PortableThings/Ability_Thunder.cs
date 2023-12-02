using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Thunder : PortableThing
{
    #region UNITY CALLBACK FUNCTIONS
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    #endregion

    public override void PickUp()
    {
        Debug.Log("學習能力: 閃電攻擊");
        PlayerAbilityManager.CanThunder = true;
        GetComponent<ThingSound>().PlayGear_PickUp();
    }
}
