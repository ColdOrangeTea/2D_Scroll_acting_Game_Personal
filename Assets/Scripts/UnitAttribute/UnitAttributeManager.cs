using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttributeManager : MonoBehaviour
{
    // 手動放入Asset
    [SerializeField] public UnitAttribute unitAttribute;
    [SerializeField] protected int maxHp = 0;
    [SerializeField] protected int hp = 0;
    [SerializeField] protected int maxMp = 0;
    [SerializeField] protected int mp = 0;
    [SerializeField] protected int attack = 0;

    void Awake()
    {
        InitAttribute();

    }

    public abstract void HpControl(int currentHp);
    public abstract void TakeDamage(int Damage);
    public abstract void Heal();
    public abstract void InitAttribute();
}
