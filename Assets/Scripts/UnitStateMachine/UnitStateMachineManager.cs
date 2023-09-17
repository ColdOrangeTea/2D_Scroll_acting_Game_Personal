using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitStateMachineManager : MonoBehaviour
{
    // 手動放入Asset
    protected UnitAttribute unitAttribute;

    // 狀態: 閒逛、攻擊、死亡
    [Header("狀態")]
    public int status = 0;
    public const int idle = 0;
    public const int attacking = 1;
    public const int dead = 2;
    [SerializeField] protected IdleState idleState;
    [SerializeField] protected AttackState attackState;

    public abstract void InitStateMachine();
}
