using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class IdleState : MonoBehaviour
{
    [SerializeField] protected UnitStateMachineManager unitStateMachineManager;
    [SerializeField] protected Rigidbody2D rb;
    // 初始面向 xAxis -1左 1右 
    [SerializeField] protected float xAxis = 1;
    // 初始面向 yAxis 1上 -1下
    [SerializeField] protected float yAxis = 0;

    [SerializeField] protected float walkSpeed = 2.5f;

    [Space(5)]

    [Header("用來檢查前方是否有牆的變數")]
    [SerializeField] protected Transform wallCheckTransform;
    [SerializeField] protected float wallCheckX;
    [SerializeField] protected float wallCheckY;
    [SerializeField] protected LayerMask groundLayer;

    [Space(5)]

    [Header("用來檢查前方是否有玩家的變數")]
    [SerializeField] protected Transform playerCheckTransform;
    [SerializeField] protected float playerCheckX;
    [SerializeField] protected float playerCheckY;
    [SerializeField] protected LayerMask playerLayer;

    void Start()
    {
        InitIdleState();
    }

    public abstract bool PlayerCheck(float moveDirection);
    public abstract void Flip();
    public abstract bool HittingWall(float moveDirection);
    public abstract IEnumerator Idle();
    public abstract void InitIdleState();
}
