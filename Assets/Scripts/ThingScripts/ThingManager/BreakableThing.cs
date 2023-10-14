using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BreakableThing : Thing
{
    #region UNITY CALLBACK FUNCTIONS
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        if (Animation.Effect.gameObject.activeInHierarchy)
        {
            if (!Animation.PS.isPlaying)
            {
                DestroyThing();
            }
        }
    }
    #endregion

    public virtual void Drop()
    {
        int thingID = Random.Range(10, 13);
        Debug.Log(thingID);
    }

    public override void TriggerThing()
    {
        Animation.Effect.gameObject.SetActive(true);
        Animation.Sprite.gameObject.SetActive(false);

        Drop();
    }
}
