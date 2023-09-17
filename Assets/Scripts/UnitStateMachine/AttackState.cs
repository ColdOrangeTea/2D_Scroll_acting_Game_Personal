using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackState : MonoBehaviour
{
    [SerializeField] protected UnitStateMachineManager unitStateMachineManager;

    [SerializeField] protected float startUp = 0;
    [SerializeField] protected float attackDuration = 10;

    // 用來計時的變數，好控制攻擊間隔
    [SerializeField] protected float attackColdDown = 19.5F;

    [SerializeField] protected Transform attackTransform;
    [SerializeField] protected LayerMask attackableLayer;

    [Space(5)]

    [Header("用來檢查前方是否有玩家的變數")]
    [SerializeField] protected Transform playerCheckTransform;
    [SerializeField] protected float playerCheckX;
    [SerializeField] protected float playerCheckY;
    [SerializeField] protected LayerMask playerLayer;

    void Start()
    {
        InitAttackState();
    }


    public abstract IEnumerator Attackperiod();
    public abstract bool PlayerCheck(float moveDirection);
    public abstract IEnumerator Attacking();
    public abstract void InitAttackState();

}
