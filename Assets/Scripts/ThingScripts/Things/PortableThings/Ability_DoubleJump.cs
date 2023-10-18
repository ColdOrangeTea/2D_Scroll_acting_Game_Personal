using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_DoubleJump : PortableThing
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
        Debug.Log("學習能力: 跳躍");
        PlayerAbilityManager.CanDoubleJump = true;
    }
}
