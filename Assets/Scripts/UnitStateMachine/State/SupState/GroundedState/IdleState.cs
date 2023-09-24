using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class IdleState : GroundedState
{
    public IdleState(PlayerMovement playerMovement, PlayerStateMachine stateMachine, UnitAttribute unitAttribute, string animBoolName) : base(playerMovement, stateMachine, unitAttribute, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (xInput != 0)
            playerMovement.CheckDirectionToFace(xInput > 0);

        if (!isExitingState)
        {
            if (xInput != 0)
            {
                stateMachine.ChangeState(playerMovement.MoveState);
            }
            // else if (yInput == -1)
            // {
            //     stateMachine.ChangeState(playerMovement.CrouchIdleState);
            // }
        }


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        playerMovement.GroundMove(1, 0, 0, unitAttribute.runAccelAmount, unitAttribute.runDeccelAmount);
    }
}


// public abstract class IdleState : MonoBehaviour
// {
//     [SerializeField] protected UnitStateMachineManager unitStateMachineManager;
//     [SerializeField] protected Rigidbody2D rb;
//     // 初始面向 xAxis -1左 1右 
//     [SerializeField] protected float xAxis = 1;
//     // 初始面向 yAxis 1上 -1下
//     [SerializeField] protected float yAxis = 0;

//     [SerializeField] protected float walkSpeed = 2.5f;

//     [Space(5)]

//     [Header("用來檢查前方是否有牆的變數")]
//     [SerializeField] protected Transform wallCheckTransform;
//     [SerializeField] protected float wallCheckX;
//     [SerializeField] protected float wallCheckY;
//     [SerializeField] protected LayerMask groundLayer;

//     [Space(5)]

//     [Header("用來檢查前方是否有玩家的變數")]

//     // 自己的BodyCollider 避免偵測到自己而攻擊
//     [SerializeField] protected Transform thisBodyTransform;
//     [SerializeField] protected Transform playerCheckTransform;
//     [SerializeField] protected float playerCheckX;
//     [SerializeField] protected float playerCheckY;
//     [SerializeField] protected LayerMask attackableLayer;

//     void Awake()
//     {
//         InitIdleState();
//     }


//     public abstract bool PlayerCheck(float moveDirection);
//     public abstract void Flip();
//     public abstract bool HittingWall(float moveDirection);
//     public abstract IEnumerator Idle();
//     public abstract void InitIdleState();
// }
