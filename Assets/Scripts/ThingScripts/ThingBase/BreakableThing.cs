using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BreakableThing : Thing
{
    [SerializeField]
    private bool IsDropped;
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
    }

    public override void TriggerThing()
    {
        Animation.Effect.gameObject.SetActive(true);
        Animation.Sprite.gameObject.SetActive(false);
        OwnCollider.isTrigger = true;
        if (!IsDropped)
        {
            IsDropped = true;
            Drop();
        }

    }
}
