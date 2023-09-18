using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMachineIdleState : IdleState
{
    // 非繼承
    private void OnTriggerEnter2D(Collider2D other)
    {

    }
    public override bool PlayerCheck(float moveDirection)
    {
        RaycastHit2D midHitInfo = Physics2D.Raycast(playerCheckTransform.position, new Vector3(moveDirection, 0), playerCheckX, attackableLayer);
        RaycastHit2D UphitInfo = Physics2D.Raycast(playerCheckTransform.position + new Vector3(0, playerCheckY), new Vector3(moveDirection, 0), playerCheckX, attackableLayer);
        RaycastHit2D DownhitInfo = Physics2D.Raycast(playerCheckTransform.position + new Vector3(0, -playerCheckY), new Vector3(moveDirection, 0), playerCheckX, attackableLayer);
        if (midHitInfo.collider != null || UphitInfo.collider != null || DownhitInfo.collider != null)
        {
            return true;
        }
        else
        {
            // Debug.Log("沒人");
            return false;
        }

        // if (Physics2D.Raycast(playerCheckTransform.position, new Vector3(moveDirection, 0), playerCheckX, attackableLayer)
        //        || Physics2D.Raycast(playerCheckTransform.position + new Vector3(0, playerCheckY), new Vector3(moveDirection, 0), playerCheckX, attackableLayer)
        //        || Physics2D.Raycast(playerCheckTransform.position + new Vector3(0, -playerCheckY), new Vector3(moveDirection, 0), playerCheckX, attackableLayer))
        // {
        //     // Debug.Log("有人在前面");

        //     return true;
        // }
        // else
        // {
        //     // Debug.Log("沒人");
        //     return false;
        // }
    }
    public override void Flip()
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
    public override bool HittingWall(float moveDirection)
    {
        // Debug.Log("方位: " + moveDirection);
        if (Physics2D.Raycast(wallCheckTransform.position, new Vector3(moveDirection, 0), wallCheckX, groundLayer)
        || Physics2D.Raycast(wallCheckTransform.position + new Vector3(0, wallCheckY), new Vector3(moveDirection, 0), wallCheckX, groundLayer)
        || Physics2D.Raycast(wallCheckTransform.position + new Vector3(0, -wallCheckY), new Vector3(moveDirection, 0), wallCheckX, groundLayer))
        {
            // Debug.Log("撞牆!!!!!!");
            return true;
        }
        else
        {
            // Debug.Log("沒有撞牆!!!!!!");
            return false;
        }
    }
    public override IEnumerator Idle()
    {
        while (unitStateMachineManager.status == UnitStateMachineManager.idle)
        {
            Debug.Log("idle");
            // 碰牆停止移動
            if (!HittingWall(xAxis))
            {
                // Debug.Log("走路");
                rb.velocity = new Vector2(xAxis * walkSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
                Flip();
            }

            if (PlayerCheck(xAxis))
            {
                rb.velocity = new Vector2(0, 0);
                // Debug.Log("idle to attacking");
                unitStateMachineManager.SwitchStatus(UnitStateMachineManager.attacking);

                // Debug.Log("idle to attacking End");
            }

            yield return null;

        }
    }
    public override void InitIdleState()
    {
        // Debug.Log("InitIdleState");
        unitStateMachineManager = GetComponent<UnitStateMachineManager>();
        rb = GetComponent<Rigidbody2D>();
        wallCheckTransform = transform.GetChild(1).GetChild(2).GetComponent<Transform>();
        playerCheckTransform = transform.GetChild(1).GetChild(3).GetComponent<Transform>();
        thisBodyTransform = transform.GetChild(1).GetChild(0).GetComponent<Transform>();
        // 初始面向 xAxis -1左 1右
        xAxis = 1;

        // 初始面向 yAxis 1上 -1下
        yAxis = 0;

        walkSpeed = 2.5f;
        wallCheckX = 1;
        wallCheckY = 0.9f;
        playerCheckX = 2;
        playerCheckY = 0.4f;
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
