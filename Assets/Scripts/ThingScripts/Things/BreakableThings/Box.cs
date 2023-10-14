using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : BreakableThing
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
    public override void Drop()
    {
        base.Drop();
        Debug.Log("噴掉落物");
        //Do something
    }
}
