using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    public PlayerData unitData;
    public Transform player;
    void Start()
    {
        unitData = FindObjectOfType<PlayerData>();
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            unitData.playerData.hp -= 1;
            Debug.Log("資料: " + unitData);
        }
    }
}
