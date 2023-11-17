using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{

    [SerializeField] private int maxHp = 20;
    [SerializeField] private int hp = 20;
    [SerializeField] private int colliderDamage = 3;
    [SerializeField] private int damage = 3;
    void Start()
    {

    }
    public void TakeDamage(int damage)
    {
        if (hp - damage > 0)
        {
            hp -= damage;

        }
        else
        {
            hp = 0;
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

    }
    public int GetColliderDamage()
    {
        return colliderDamage;
    }
}
