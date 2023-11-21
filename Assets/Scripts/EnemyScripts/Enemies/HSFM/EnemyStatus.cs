using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyStatus : MonoBehaviour
{

    [SerializeField] private int maxHp;
    [SerializeField] private int hp;

    #region  DAMAGE PARAMETER
    [Header("TakeDamage")]
    [SerializeField] private int colliderDamage = 3;
    [SerializeField] private int damage = 3;
    private Coroutine hurtForceRoutine;
    [SerializeField] private float hurtForceDuration;
    public UnityEvent OnTakeDamage;
    #endregion

    void Start()
    {
        hp = maxHp;
    }
    public void HurtForce()
    {
        if (hurtForceRoutine != null)
            StartCoroutine(FlashRoutine());

        Debug.Log("Enemy Hurt.");
    }
    private IEnumerator FlashRoutine()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(hurtForceDuration);
        hurtForceRoutine = null;
    }
    public void HurtAnimator()
    {
        Animator animator = GetComponentInChildren<Animator>();
        animator.SetTrigger("Hurt");
        // Debug.Log(" animator.SetTrigger");
    }
    public void TakeDamage(int damage)
    {
        if (hp - damage > 0)
        {
            hp -= damage;
            OnTakeDamage?.Invoke();
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
