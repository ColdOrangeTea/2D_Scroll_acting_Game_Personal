using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState CurrentState { get; private set; }

    public void Initialize(EnemyState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
        Debug.Log(CurrentState);
    }

    public void ChangeState(EnemyState newState)
    {
        CurrentState.Exit();
        Debug.Log("敵人 " + CurrentState + " Changeinto" + " " + newState);
        CurrentState = newState;
        CurrentState.Enter();

    }
}
