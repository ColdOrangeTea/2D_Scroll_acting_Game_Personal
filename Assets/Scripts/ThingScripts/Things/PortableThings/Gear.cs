using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : PortableThing
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
        base.PickUp();
        LevelManager.NumOfGears += 1;
        GetComponent<ThingSound>().PlayGear_PickUp();
        Debug.Log("撿取物品: 齒輪+1 ，目前數量:" + LevelManager.NumOfGears);
    }
}
