using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandState : GroundState
{
    public LandState(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyAttribute enemyAttribute, string anim_bool_name) : base(enemy, enemyStateMachine, enemyAttribute, anim_bool_name)
    {
    }
}
