using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy0001 : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform checkCenter;
    [SerializeField] private float xAxis;
    [SerializeField] private float yAxis;

    // 狀態: 閒逛、攻擊、死亡
    [Header("狀態")]
    private int status = 0;
    private const int idle = 0;
    private const int attacking = 1;
    private const int dead = 2;

    [Space(5)]

    [Header("閒逛")]
    [SerializeField] private float walkSpeed = 0.8f;

    [Space(5)]

    [Header("用來檢查前方是否有牆的變數")]
    [SerializeField] private Transform wallCheckTransform;
    [SerializeField] private float wallCheckX;
    [SerializeField] private float wallCheckY;
    [SerializeField] private LayerMask groundLayer;

    [Space(5)]

    [Header("攻擊Check")]
    [SerializeField]
    private float attackingCheckRadius = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wallCheckTransform = transform.GetChild(1).GetChild(0).GetComponent<Transform>();
        checkCenter = GetComponent<Transform>();
        status = 0;
    }

    void Update()
    {
        Idle();
    }

    // 角色轉向
    void Flip()
    {
        if (xAxis > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else if (xAxis < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
    }

    public bool HittingWall()
    {
        if (Physics2D.Raycast(wallCheckTransform.position, Vector2.right, wallCheckX, groundLayer)
        || Physics2D.Raycast(wallCheckTransform.position + new Vector3(0, wallCheckY), Vector2.right, wallCheckX, groundLayer)
        || Physics2D.Raycast(wallCheckTransform.position + new Vector3(0, -wallCheckY), Vector2.right, wallCheckX, groundLayer))
        {
            Debug.Log("撞牆!!!!!!");
            return true;
        }
        else
        {
            Debug.Log("沒有撞牆!!!!!!");
            return false;
        }
    }

    // 狀態互換: idle <=> attack 、 idle <=> dead
    void Idle()
    {
        if (status == idle)
        {

            Debug.Log("牆: " + HittingWall());
            if (HittingWall())
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else if (!HittingWall())
            {
                Debug.Log("走路");
                rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
            }
        }


    }


    // 狀態互換: attacking <=> idle 、 attacking <=> dead
    void Attacking()
    {

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheckTransform.position, wallCheckTransform.position + new Vector3(wallCheckX, 0));
        Gizmos.DrawLine(wallCheckTransform.position + new Vector3(0, wallCheckY), wallCheckTransform.position + new Vector3(wallCheckX, wallCheckY));
        Gizmos.DrawLine(wallCheckTransform.position + new Vector3(0, -wallCheckY), wallCheckTransform.position + new Vector3(wallCheckX, -wallCheckY));

    }
}
