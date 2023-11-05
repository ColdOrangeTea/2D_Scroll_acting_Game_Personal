using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : AbilityState
{
    private bool ranged_attack_used;
    private int ranged_attack_counts;
    private Vector2 ranged_attack_direction;
    private float last_ranged_attack_time;
    public RangedAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyAttribute enemyAttribute, string anim_bool_name) : base(enemy, enemyStateMachine, enemyAttribute, anim_bool_name)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        enemy.EnemyPhysicCheck.CheckHittedUnit();
    }

    public override void Enter()
    {
        base.Enter();

        ranged_attack_used = false;
        ranged_attack_counts = 0;
        // enemy.RB.drag = enemyAttribute.MeleeDrag;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            if (CheckIfCanAttack())
            {
                // Debug.Log("Time.time: " + Time.time + " 經過時間: " + last_melee_attack_time + " " + enemyAttribute.MeleeCooldown);

                if (Time.time >= startTime + enemyAttribute.RangeAttackDuration)
                {
                    if (ranged_attack_used)
                    {
                        // Debug.Log("攻擊 時間: " + Time.time + " 攻擊時: " + startTime + "攻擊時長:" + enemyAttribute.MeleeDuration);
                        isAbilityDone = true;
                        last_ranged_attack_time = Time.time;
                        Debug.Log("攻擊完畢");

                    }
                }
                else
                {
                    if (ranged_attack_used)
                        Debug.Log("目前處在: 攻擊結束後~脫離攻擊狀態 的空檔");
                }

                if (!ranged_attack_used)
                {
                    enemy.MeleeAttack();
                    ranged_attack_used = true;
                    startTime = Time.time;
                    Debug.Log("攻擊的當下");

                }
            }
            else
            {
                Debug.Log("在攻擊狀態內，但在冷卻中");
            }

        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public bool CheckIfCanAttack()
    {
        if (last_ranged_attack_time == 0) // 時間 = 0 代表敵人初次觸發攻擊
            return true;
        else
            return Time.time >= last_ranged_attack_time + enemyAttribute.RangeAttackCooldown;
    }

}
