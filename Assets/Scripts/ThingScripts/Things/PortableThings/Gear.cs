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

        LevelManager.NumofGears += 1;
        Debug.Log("撿取物品: 齒輪+1 ，目前數量:" + LevelManager.NumofGears);
    }
}
