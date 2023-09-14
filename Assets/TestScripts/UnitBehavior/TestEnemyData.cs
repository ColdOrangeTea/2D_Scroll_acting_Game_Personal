using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyData : MonoBehaviour
{
    public UnitData enemyData;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HpControl(enemyData.hp);
    }
    void HpControl(int currentHp)
    {
        if (currentHp > enemyData.maxHp)
        {
            currentHp = enemyData.maxHp;
        }
        if (currentHp == 0)
        {

        }
        else if (currentHp < 0)
        {
            currentHp = 0;
        }
        enemyData.hp = currentHp;
    }
}
