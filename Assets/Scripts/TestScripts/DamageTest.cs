using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    public PlayerAttribute playerAttribute;
    void Start()
    {
        playerAttribute = FindObjectOfType<PlayerAttribute>();
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        playerAttribute.TakeDamage();

    //        Debug.Log("資料: " + playerAttribute);
    //    }
    //}
}
