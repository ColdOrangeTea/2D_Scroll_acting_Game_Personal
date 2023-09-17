using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackState : MonoBehaviour
{
    [SerializeField] private UnitStateMachineManager unitStateMachineManager;

    [SerializeField] private float attackDuration = 10;

    // 用來計時的變數，好控制攻擊間隔
    [SerializeField] private float attackColdDown = 19.5F;

    [SerializeField] private Transform attackTransform;
    [SerializeField] private LayerMask attackableLayer;

    [Space(5)]

    [Header("用來檢查前方是否有玩家的變數")]
    [SerializeField] private Transform playerCheckTransform;
    [SerializeField] private float playerCheckX;
    [SerializeField] private float playerCheckY;
    [SerializeField] private LayerMask playerLayer;

    void Start()
    {
        InitAttackState();
    }


    public abstract IEnumerator Attackperiod();
    public abstract bool PlayerCheck(float moveDirection);
    public abstract IEnumerator Attacking();
    public abstract void InitAttackState();

}
