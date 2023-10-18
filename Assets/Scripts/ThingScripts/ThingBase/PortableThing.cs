using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortableThing : Thing
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

    public virtual void PickUp()
    {
        Debug.Log("撿取物品");
        //Do something
    }

    public override void TriggerThing()
    {
        PickUp();
        DestroyThing();
    }

    // public virtual void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(OwnCollider.IsTouching(other))
    //     if (other.CompareTag(PLAYER))
    //     {
    //         PickUp();
    //         DestroyThing();
    //     }
    // }
}
