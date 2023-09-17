using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMachineStateMachineManager : UnitStateMachineManager
{
    // 測試用
    public EnemyStatusTest enemyStatusTest;

    void Start()
    {
        InitStateMachine();
    }

    void Update()
    {

    }

    public override void SwitchStatus(int nextStatus)
    {
        status = nextStatus;
        // 測試用
        enemyStatusTest.UpdateEnemyStatus(status);
        if (status == idle)
        {
            StartCoroutine(idleState.Idle());
        }
        else if (status == attacking)
        {
            StartCoroutine(attackState.Attacking());
        }

    }

    public override void InitStateMachine()
    {
        status = 0;
        idleState = GetComponent<IdleState>();
        attackState = GetComponent<AttackState>();

        // 測試用
        enemyStatusTest = FindObjectOfType<EnemyStatusTest>();

        StartCoroutine(idleState.Idle());

        // 測試用
        enemyStatusTest.UpdateEnemyStatus(status);
    }
}
