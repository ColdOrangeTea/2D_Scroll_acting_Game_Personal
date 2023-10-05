using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMachine : Enemy
{
    [SerializeField]
    private EnemyAttribute meleeMachine_attribute;
    // [SerializeField] private Core core;

    protected override void Awake()
    {
        // core = GetComponent<Core>();
        // Core = core;
        enemyAttribute = meleeMachine_attribute;
        Debug.Log("賦予資料" + enemyAttribute);
        base.Awake();

    }
}
