using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeadState : MonoBehaviour
{
    [SerializeField] protected UnitStateMachineManager unitStateMachineManager;
    [SerializeField] protected GameObject thisThing;

    void Awake()
    {
        InitDeadState();
    }
    public abstract IEnumerator Dying();
    public abstract void InitDeadState();
}
