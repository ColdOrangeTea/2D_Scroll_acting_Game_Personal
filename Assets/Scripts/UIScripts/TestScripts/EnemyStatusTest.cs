using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyStatusTest : MonoBehaviour
{
    public Text statusText;
    private string statusName = "";
    void Start()
    {

    }
    void Update()
    {

    }

    public void UpdateEnemyStatus(int status)
    {
        if (status == 0)
        {
            statusName = "Idle";
        }
        else if (status == 1)
        {
            statusName = "Attack";
        }
        statusText.text = "Status: " + status.ToString() + " " + statusName;
    }

}
