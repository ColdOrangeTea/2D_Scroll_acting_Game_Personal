using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour
{
    #region COMPONENT
    public Collider2D OwnCollider;//{ get; private set; }    
    public ThingAnimation Animation;//{ get; private set; }
    public ThingPhysicsCheck PhysicsCheck;//{ get; private set; }

    public ThingAttribute Attribute;
    #endregion

    protected virtual void Start()
    {
        OwnCollider = GetComponentInChildren<Collider2D>();
        Animation = GetComponentInChildren<ThingAnimation>();
        PhysicsCheck = GetComponentInChildren<ThingPhysicsCheck>();
        Animation.Effect.gameObject.SetActive(false);
    }


    protected virtual void Update()
    {
    }

    public virtual void DestroyThing()
    {
        this.gameObject.SetActive(false);
        Destroy(this);
    }

    public virtual void TakeDamage()
    {

    }

}
