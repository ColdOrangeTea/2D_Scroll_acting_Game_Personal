using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musketeer : Enemy
{
    [SerializeField]
    private EnemyAttribute musketeer_attribute;
    // [SerializeField] private Core core;

    protected override void Awake()
    {
        // core = GetComponent<Core>();
        // Core = core;
        enemyAttribute = musketeer_attribute;
        Debug.Log("賦予資料" + enemyAttribute);
        base.Awake();

    }
}
