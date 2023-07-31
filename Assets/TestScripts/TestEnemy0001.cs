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
    [SerializeField] private float walkSpeed = 5;

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


    [Header("用來檢查是否踩著地板的變數")]

    // 在玩家碰撞格下方的 transform 型別的 childed
    [SerializeField] private Transform groundTransform;

    // 在Y軸上的 groundcheck 的 Raycast長度
    [SerializeField] private float groundCheckY = 0.2f;

    // 在X軸上的 groundcheck 的 Raycast長度
    [SerializeField] private float groundCheckX = 0.25f;
    public bool Grounded()
    {
        //用在不同位置的三個射線檢查玩家角色有沒有踩在地板
        if (Physics2D.Raycast(groundTransform.position, Vector2.down, groundCheckY, groundLayer)
        || Physics2D.Raycast(groundTransform.position + new Vector3(-groundCheckX, 0), Vector2.down, groundCheckY, groundLayer)
        || Physics2D.Raycast(groundTransform.position + new Vector3(groundCheckX, 0), Vector2.down, groundCheckY, groundLayer))
        {
            Debug.Log("在地板");
            return true;
        }
        else
        {
            Debug.Log("不在地板");
            return false;
        }
    }
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

    // public bool HittingWall()
    // {
    // if (Physics.Raycast(wallCheckTransform.position, , wallCheckX, groundLayer)
    // || Physics.Raycast(wallCheckTransform.position + new Vector3(0, wallCheckY), Vector2.right, wallCheckX, groundLayer)
    // || Physics.Raycast(wallCheckTransform.position + new Vector3(0, -wallCheckY), Vector2.right, wallCheckX, groundLayer))
    // {
    //     Debug.Log("撞牆!!!!!!");
    //     return true;
    // }
    // else
    // {
    //     Debug.Log("沒有撞牆!!!!!!");
    //     return false;
    // }
    // }

    // 狀態互換: idle <=> attack 、 idle <=> dead
    void Idle()
    {
        if (status == idle)
        {
            if (Grounded())
            {
                Debug.Log("採");
            }
            else
            {
                Debug.Log("在空中");
            }
            // Debug.Log("牆: " + HittingWall());
            // if (HittingWall())
            // {
            //     Debug.Log("撞牆");
            // }
            // else
            // {
            //     Debug.Log("走路");
            //     rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
            // }
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


        Gizmos.DrawLine(groundTransform.position, groundTransform.position + new Vector3(0, -groundCheckY));
        Gizmos.DrawLine(groundTransform.position + new Vector3(-groundCheckX, 0), groundTransform.position + new Vector3(-groundCheckX, -groundCheckY));
        Gizmos.DrawLine(groundTransform.position + new Vector3(groundCheckX, 0), groundTransform.position + new Vector3(groundCheckX, -groundCheckY));

    }
}
