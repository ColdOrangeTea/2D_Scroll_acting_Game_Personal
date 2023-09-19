using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 這個腳本用來控制玩家(主角)的移動、攻擊等能力
public class PlayerMovement : MonoBehaviour
{
    public PlayerStateList pState;

    [SerializeField]
    private Rigidbody2D rb;

    [Header("X軸 移動")]
    [SerializeField]
    private float walkSpeed = 10;

    [Space(5)]

    [Header("Y軸 移動")]

    [SerializeField] private float jumpSpeed = 12;
    [SerializeField] private float fallSpeed = 45;

    // 跳躍的時間限制
    [SerializeField] private int jumpSteps = 20;
    // 跳躍的臨界點，在臨界點時間內放掉
    [SerializeField] private int jumpThreshold = 1;

    [Header("攻擊")]
    [SerializeField] private float coldDown = 0.8f;
    [SerializeField] private float AttackPeriod = 0.4f;
    // transform 型別的 childed 主角正面攻擊的碰撞格
    [SerializeField] private Transform attackTransform;

    // transform 型別的 childed 朝下攻擊的碰撞格
    [SerializeField] private Transform downAttackTransform;

    // transform 型別的 childed 朝上攻擊的碰撞格
    [SerializeField] private Transform upAttackTransform;

    [SerializeField] private LayerMask attackableLayer;
    [Space(5)]

    // [Header("Recoil 反作用力")]
    // [SerializeField] private int recoilXSteps = 4;
    // [SerializeField] private int recoilYSteps = 10;
    // [SerializeField] private float recoilXSpeed = 45;
    // [SerializeField] private float recoilYSpeed = 45;
    // [Space(5)]

    [Header("用來檢查是否踩著地板的變數")]

    // 在玩家碰撞格下方的 transform 型別的 childed
    [SerializeField] private Transform groundTransform;

    // 在Y軸上的 groundcheck 的 Raycast長度
    [SerializeField] private float groundCheckY = 0.2f;

    // 在X軸上的 groundcheck 的 Raycast長度
    [SerializeField] private float groundCheckX = 0.25f;

    [SerializeField] private LayerMask groundLayer;
    [Space(5)]

    [Header("用來檢查頭是否撞到頂的變數")]

    // 在玩家碰撞格上方的 transform 型別的 childed
    [SerializeField] private Transform roofTransform;

    // 這下面兩個變數的用途和 groundCheckX、groundCheckY一樣，但這是用來檢查頭頂的
    [SerializeField] private float roofCheckY = 0.2f;
    [SerializeField] private float roofCheckX = 0.25f;


    [Space(20)]
    [SerializeField]
    float xAxis;
    [SerializeField]
    float yAxis;
    // [SerializeField]
    // float gravity;
    // [SerializeField]
    // int stepsXRecoiled;
    // [SerializeField]
    // int stepsYRecoiled;

    // 玩家按下跳躍鍵時，用來計時的變數，好控制逐漸增加的Y軸高度(小於jumpSteps、jumpThreshold)
    [SerializeField]
    private int stepsJumped = 0;
    // 玩家按下攻擊鍵時，用來計時的變數，好控制攻擊間隔
    [SerializeField]
    private float coldDownSinceAttack = 0;
    [SerializeField]
    private float timeSinceAttack = 0;

    void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Flip();
        Walk(xAxis);
        GetInputs();
        AttackColdDown();
        Attack(yAxis);
    }

    void FixedUpdate()
    {
        Jump();
    }

    void AttackColdDown()
    {
        if (IsAttackColdDown())
        {
            // 攻擊的間隔
            coldDownSinceAttack -= Time.deltaTime;
            if (coldDownSinceAttack <= 0)
            {
                coldDownSinceAttack = 0;
            }
        }

    }

    // false代表不在冷卻狀態，可進行攻擊 true代表還在冷卻
    public bool IsAttackColdDown()
    {
        if (coldDownSinceAttack == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

   

    void Attack(float faceDirection)
    {
        if (pState.attacking)
        {
            timeSinceAttack += Time.deltaTime;

            // 攻擊是否處在冷卻狀態，若否即可攻擊
            if (!IsAttackColdDown())
            {
                // 進行攻擊冷卻時間的計算
                coldDownSinceAttack = coldDown;

                if (timeSinceAttack < AttackPeriod)
                {
                    // 方向攻擊
                    if (faceDirection == 1)
                    {
                        upAttackTransform.gameObject.SetActive(true);
                    }
                    if (faceDirection == -1)
                    {
                        downAttackTransform.gameObject.SetActive(true);
                    }
                    if (faceDirection == 0)
                    {
                        attackTransform.gameObject.SetActive(true);
                    }
                }
            }

            if (timeSinceAttack >= AttackPeriod)
            {
                timeSinceAttack = 0;
                pState.attacking = false;
                upAttackTransform.gameObject.SetActive(false);
                downAttackTransform.gameObject.SetActive(false);
                attackTransform.gameObject.SetActive(false);
            }
        }
    }

    // 角色轉向
    void Flip()
    {
        if (!pState.attacking)
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

    }

    void Walk(float moveDirection)
    {
        if (!pState.attacking)
        {
            rb.velocity = new Vector2(moveDirection * walkSpeed, rb.velocity.y);
        }
        else
        {
            if (rb.velocity.y == 0)
                rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (Mathf.Abs(rb.velocity.x) > 0)
        {
            pState.walking = true;
        }
        else
        {
            pState.walking = false;
        }
    }
    void Jump()
    {
        // if (!pState.attacking)
        // {
        if (pState.jumping)
        {
            // 還沒到跳躍的限制，也沒有撞到天花板就可以繼續往上跳
            if (stepsJumped < jumpSteps && !Roofed())
            {
                // Debug.Log("繼續往上跳");
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                stepsJumped++;
                // Debug.Log("rb.velocity = " + rb.velocity);
            }
            else
            {
                // Debug.Log("停止跳躍");
                StopJumpSlow();
            }
        }
        // 玩家掉落的速度限制 避免掉太快穿過平台 要限制速度
        if (rb.velocity.y < -Mathf.Abs(fallSpeed))
        {
            // Debug.Log("限制跳躍速度");
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -Mathf.Abs(fallSpeed), Mathf.Infinity));
        }
        // }
    }

    // 玩家放開按鍵時立刻停止跳躍，讓角色開始落下
    void StopJumpQuick()
    {
        // Debug.Log("停止跳躍，向上的力立刻歸0");
        stepsJumped = 0;
        pState.jumping = false;
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    //停止跳躍(增加Y軸)
    void StopJumpSlow()
    {
        stepsJumped = 0;
        pState.jumping = false;
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
        // Debug.Log("rb.velocity = " + rb.velocity);
    }

    public bool Grounded()
    {
        //用在不同位置的三個射線檢查玩家角色有沒有踩在地板
        if (Physics2D.Raycast(groundTransform.position, Vector2.down, groundCheckY, groundLayer)
        || Physics2D.Raycast(groundTransform.position + new Vector3(-groundCheckX, 0), Vector2.down, groundCheckY, groundLayer)
        || Physics2D.Raycast(groundTransform.position + new Vector3(groundCheckX, 0), Vector2.down, groundCheckY, groundLayer))
        {
            // Debug.Log("在地板");
            return true;
        }
        else
        {
            // Debug.Log("不在地板");
            return false;
        }
    }
    public bool Roofed()
    {
        // 檢查玩家的角色頭有沒有撞到障礙物/頂
        // 用來取消跳躍狀態
        if (Physics2D.Raycast(roofTransform.position, Vector2.up, roofCheckY, groundLayer)
        || Physics2D.Raycast(roofTransform.position + new Vector3(-roofCheckX, 0), Vector2.up, roofCheckY, groundLayer)
        || Physics2D.Raycast(roofTransform.position + new Vector3(roofCheckX, 0), Vector2.up, roofCheckY, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void GetInputs()
    {
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");

        // 1 > x軸輸入 > 0 往東
        // 0 > x軸輸入 > -1 往西
        if (xAxis > 0.1)
        {
            xAxis = 1;
        }
        else if (xAxis < -0.1)
        {
            xAxis = -1;
        }
        else
        {
            xAxis = 0;
        }

        // 1 > y軸輸入 > 0 往北
        // 0 > y軸輸入 > -1 往南
        if (yAxis > 0.1)
        {
            yAxis = 1;
        }
        else if (yAxis < -0.1)
        {
            yAxis = -1;
        }
        else
        {
            yAxis = 0;
        }

        // 輸入攻擊鍵
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!IsAttackColdDown())
            {
                pState.attacking = true;
                Debug.Log("攻擊");
            }
        }

        // 在地板輸入跳躍鍵
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (Grounded())
            {
                pState.jumping = true;
            }
            // Debug.Log("跳起來 " + pState.jumping);
        }

        // 按超過時間的臨界點，但還沒跳到最高點時放開跳躍鍵
        if (!Input.GetKey(KeyCode.Z) && stepsJumped < jumpSteps && stepsJumped > jumpThreshold && pState.jumping)
        {
            // Debug.Log("向上的力立刻歸0");
            StopJumpQuick();
        }
        else if (!Input.GetKey(KeyCode.Z) && stepsJumped < jumpThreshold && pState.jumping)
        {
            // Debug.Log("停止上升");
            StopJumpSlow();
        }

        // 輸入攻擊鍵
        if (Input.GetKeyDown(KeyCode.X))
        {

        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(attackTransform.position, attackRadius);
        // Gizmos.DrawWireSphere(downAttackTransform.position, downAttackRadius);
        // Gizmos.DrawWireSphere(upAttackTransform.position, upAttackRadius);

        // Gizmos.DrawWireCube(groundTransform.position, new Vector2(groundCheckX, groundCheckY));

        Gizmos.DrawLine(groundTransform.position, groundTransform.position + new Vector3(0, -groundCheckY));
        Gizmos.DrawLine(groundTransform.position + new Vector3(-groundCheckX, 0), groundTransform.position + new Vector3(-groundCheckX, -groundCheckY));
        Gizmos.DrawLine(groundTransform.position + new Vector3(groundCheckX, 0), groundTransform.position + new Vector3(groundCheckX, -groundCheckY));

        Gizmos.DrawLine(roofTransform.position, roofTransform.position + new Vector3(0, roofCheckY));
        Gizmos.DrawLine(roofTransform.position + new Vector3(-roofCheckX, 0), roofTransform.position + new Vector3(-roofCheckX, roofCheckY));
        Gizmos.DrawLine(roofTransform.position + new Vector3(roofCheckX, 0), roofTransform.position + new Vector3(roofCheckX, roofCheckY));
    }

}