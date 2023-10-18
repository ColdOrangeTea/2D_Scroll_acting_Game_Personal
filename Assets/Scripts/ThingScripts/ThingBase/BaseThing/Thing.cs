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
    #region TAG NAME
    public const string PLAYER = "Player";
    public const string ENEMY = "Enemy";
    #endregion
    #region UNITY CALLBACK FUNCTIONS
    protected virtual void Start()
    {
        OwnCollider = GetComponent<Collider2D>();
        Animation = GetComponentInChildren<ThingAnimation>();
        PhysicsCheck = GetComponentInChildren<ThingPhysicsCheck>();
    }
    protected virtual void Update()
    {
    }
    #endregion

    public virtual void DestroyThing()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    public virtual void TriggerThing()
    {
    }

}
