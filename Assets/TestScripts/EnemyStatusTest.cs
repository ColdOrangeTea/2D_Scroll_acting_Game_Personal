using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyStatusTest : MonoBehaviour
{
    public TestEnemy0001 enemy;
    public Text status;
    private string statusName;
    void Start()
    {

    }

    void Update()
    {
        if (enemy.status == 0)
        {
            statusName = "Idle";
        }
        else if (enemy.status == 1)
        {
            statusName = "Attack";
        }
        status.text = "Status: " + enemy.status.ToString() + " " + statusName;
    }
}
