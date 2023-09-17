using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMachineStateMachineManager : UnitStateMachineManager
{

    void Start()
    {
        InitStateMachine();
    }

    void Update()
    {

    }

    public override void InitStateMachine()
    {
        status = 0;
        idleState = GetComponent<IdleState>();
        attackState = GetComponent<AttackState>();

        StartCoroutine(idleState.Idle());
    }
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(wallCheckTransform.position, wallCheckTransform.position + new Vector3(xAxis * wallCheckX, 0));
    //     Gizmos.DrawLine(wallCheckTransform.position + new Vector3(0, wallCheckY), wallCheckTransform.position + new Vector3(xAxis * wallCheckX, wallCheckY));
    //     Gizmos.DrawLine(wallCheckTransform.position + new Vector3(0, -wallCheckY), wallCheckTransform.position + new Vector3(xAxis * wallCheckX, -wallCheckY));

    //     Gizmos.DrawLine(playerCheckTransform.position, playerCheckTransform.position + new Vector3(xAxis * playerCheckX, 0));
    //     Gizmos.DrawLine(playerCheckTransform.position + new Vector3(0, playerCheckY), playerCheckTransform.position + new Vector3(xAxis * playerCheckX, playerCheckY));
    //     Gizmos.DrawLine(playerCheckTransform.position + new Vector3(0, -playerCheckY), playerCheckTransform.position + new Vector3(xAxis * playerCheckX, -playerCheckY));

    // }
}
