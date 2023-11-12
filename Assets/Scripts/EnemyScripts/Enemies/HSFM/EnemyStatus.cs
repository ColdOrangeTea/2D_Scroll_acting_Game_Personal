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
    public int GetColliderDamage()
    {
        return colliderDamage;
    }
}
