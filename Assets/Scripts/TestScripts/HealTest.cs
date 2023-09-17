using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTest : MonoBehaviour
{
    public PlayerAttribute playerAttribute;
    void Start()
    {
        playerAttribute = FindObjectOfType<PlayerAttribute>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerAttribute.Heal();

            Debug.Log("資料: " + playerAttribute);
        }
    }
}
