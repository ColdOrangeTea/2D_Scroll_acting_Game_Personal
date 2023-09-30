using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region STATE VARIABLES
    public Core Core { get; private set; }
    public EnemyStateMachine EnemyStateMachine { get; private set; }
    public IdleState IdleState { get; private set; }
    public MoveState MoveState { get; private set; }
    public JumpState JumpState { get; private set; }
    public MeleeAttackState MeleeAttackState { get; private set; }
    public PlayerDashState PlayerDashState { get; private set; }
    public InAirState InAirState { get; private set; }
    public LandState LandState { get; private set; }

    [SerializeField]
    private EnemyAttribute enemy_attribute;

    #endregion


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
