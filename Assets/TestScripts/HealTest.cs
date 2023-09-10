using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTest : MonoBehaviour
{
    public PlayerData unitData;
    public Transform player;
    void Start()
    {
        unitData = FindObjectOfType<PlayerData>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("碰到補血");
            unitData.playerData.hp += 1;
            Debug.Log("資料: " + unitData);
        }
    }
}
