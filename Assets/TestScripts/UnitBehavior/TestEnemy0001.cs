using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy0001 : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform checkCenter;

    // 初始面向 xAxis -1左 1右 
    [SerializeField] private float xAxis = 1;
    // 初始面向 yAxis 1上 -1下
    [SerializeField] private float yAxis = 0;

    // 狀態: 閒逛、攻擊、死亡
    [Header("狀態")]
    private int status = 0;
    private const int idle = 0;
    private const int attacking = 1;
    private const int dead = 2;

    [Space(5)]

    [Header("閒逛")]
    [SerializeField] private float walkSpeed = 2.5f;

    [Space(5)]

    [Header("用來檢查前方是否有牆的變數")]
    [SerializeField] private Transform wallCheckTransform;
    [SerializeField] private float wallCheckX;
    [SerializeField] private float wallCheckY;
    [SerializeField] private LayerMask groundLayer;

    [Space(5)]

    [Header("用來檢查前方是否有玩家的變數")]
    [SerializeField] private Transform playerCheckTransform;
    [SerializeField] private float playerCheckX;
    [SerializeField] private float playerCheckY;
    [SerializeField] private LayerMask playerLayer;

    [Space(5)]

    [Header("攻擊Check")]
    [SerializeField]
    private float attackingCheckRadius = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        xAxis = 1;
        wallCheckTransform = transform.GetChild(1).GetChild(0).GetComponent<Transform>();
        checkCenter = GetComponent<Transform>();
        status = 0;

    }

    void Update()
    {
        Idle(xAxis);
        Attacking();
    }

    // 狀態互換: attacking <=> idle 、 attacking <=> dead
    void Attacking()
    {
        if (status == attacking)
        {
            Debug.Log("打人喔!!");
            // 打人
        }

    }

    public bool PlayerCheck(float moveDirection)
    {
        if (Physics2D.Raycast(playerCheckTransform.position, new Vector3(moveDirection, 0), playerCheckX, playerLayer)
        || Physics2D.Raycast(playerCheckTransform.position + new Vector3(0, playerCheckY), new Vector3(moveDirection, 0), playerCheckX, playerLayer)
        || Physics2D.Raycast(playerCheckTransform.position + new Vector3(0, -playerCheckY), new Vector3(moveDirection, 0), playerCheckX, playerLayer))
        {
            Debug.Log("有人在前面");
            return true;
        }
        else
        {
            Debug.Log("沒人");
            return false;
        }
    }

    // 角色轉向
    void Flip()
    {
        // 如果這個角色當下的面向朝右，則往左轉向(回頭)
        if (xAxis > 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            xAxis = -1;
        }
        else if (xAxis < 0)         // 如果這個角色當下的面向朝左，則往右轉向(回頭)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            xAxis = 1;
        }
    }

    public bool HittingWall(float moveDirection)
    {
        Debug.Log("方位: " + moveDirection);
        if (Physics2D.Raycast(wallCheckTransform.position, new Vector3(moveDirection, 0), wallCheckX, groundLayer)
        || Physics2D.Raycast(wallCheckTransform.position + new Vector3(0, wallCheckY), new Vector3(moveDirection, 0), wallCheckX, groundLayer)
        || Physics2D.Raycast(wallCheckTransform.position + new Vector3(0, -wallCheckY), new Vector3(moveDirection, 0), wallCheckX, groundLayer))
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
    void Idle(float moveDirection)
    {
        if (PlayerCheck(xAxis))
        {
            rb.velocity = new Vector2(0, 0);
            status = attacking;
        }

        if (status == idle)
        {
            // 碰牆停止移動
            if (!HittingWall(xAxis))
            {
                Debug.Log("走路");
                rb.velocity = new Vector2(moveDirection * walkSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
                Flip();
            }

        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheckTransform.position, wallCheckTransform.position + new Vector3(xAxis * wallCheckX, 0));
        Gizmos.DrawLine(wallCheckTransform.position + new Vector3(0, wallCheckY), wallCheckTransform.position + new Vector3(xAxis * wallCheckX, wallCheckY));
        Gizmos.DrawLine(wallCheckTransform.position + new Vector3(0, -wallCheckY), wallCheckTransform.position + new Vector3(xAxis * wallCheckX, -wallCheckY));

        Gizmos.DrawLine(playerCheckTransform.position, playerCheckTransform.position + new Vector3(xAxis * playerCheckX, 0));
        Gizmos.DrawLine(playerCheckTransform.position + new Vector3(0, playerCheckY), playerCheckTransform.position + new Vector3(xAxis * playerCheckX, playerCheckY));
        Gizmos.DrawLine(playerCheckTransform.position + new Vector3(0, -playerCheckY), playerCheckTransform.position + new Vector3(xAxis * playerCheckX, -playerCheckY));


    }
}