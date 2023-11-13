using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int colliderDamage = 3;
    enum NumOfLayer
    {
        AttackableUnit = 7,
        Thing = 8,
        Attack = 9
    }
    public const string PLAYER = "Player";

    void Start()
    {
        Destroy(gameObject,1);
    }
    public int GetColliderDamage()
    {
        return colliderDamage;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == (int)NumOfLayer.AttackableUnit)
        {
            if (other.gameObject.CompareTag(PLAYER))
            {
                this.gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
