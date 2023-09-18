using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttributeManager : MonoBehaviour
{
    // 手動放入Asset
    [SerializeField] protected UnitAttribute unitAttribute;

    void Awake()
    {
        InitAttribute(); ;
    }

    public abstract void HpControl(int currentHp);
    public abstract void TakeDamage();
    public abstract void Heal();
    public abstract void InitAttribute();
}
