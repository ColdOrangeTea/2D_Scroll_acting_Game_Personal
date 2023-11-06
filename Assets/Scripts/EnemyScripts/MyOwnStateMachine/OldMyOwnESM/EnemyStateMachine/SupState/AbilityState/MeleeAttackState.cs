using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttackState : AbilityState
{
    private bool melee_attack_used;
    private int melee_attack_counts;
    private Vector2 melee_attack_direction;
    private float last_melee_attack_time;
    public MeleeAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine, EnemyAttribute enemyAttribute, string anim_bool_name) : base(enemy, enemyStateMachine, enemyAttribute, anim_bool_name)
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

        melee_attack_used = false;
        melee_attack_counts = 0;
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

                if (Time.time >= startTime + enemyAttribute.MeleeDuration)
                {
                    if (melee_attack_used)
                    {
                        // Debug.Log("攻擊 時間: " + Time.time + " 攻擊時: " + startTime + "攻擊時長:" + enemyAttribute.MeleeDuration);
                        isAbilityDone = true;
                        last_melee_attack_time = Time.time;
                        Debug.Log("攻擊完畢");

                    }
                }
                else
                {
                    if (melee_attack_used)
                        Debug.Log("目前處在: 攻擊結束後~脫離攻擊狀態 的空檔");
                }

                if (!melee_attack_used)
                {
                    enemy.MeleeAttack();
                    melee_attack_used = true;
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

        // if (isGrounded)
        // {
        //     enemy.GroundMove(1, xInput, enemyAttribute.MoveMaxSpeed, enemyAttribute.MoveAccelAmount, enemyAttribute.MoveDeccelAmount);
        // }
        // else
        //     enemy.InAirMove(1, xInput, enemyAttribute.MoveMaxSpeed, enemyAttribute.MoveAccelAmount * enemyAttribute.AccelInAir,
        //     enemyAttribute.MoveDeccelAmount * enemyAttribute.DeccelInAir, enemyAttribute.JumpHangTimeThreshold,
        //     enemyAttribute.JumpHangAccelerationMult, enemyAttribute.jumpHangMaxSpeedMult, enemyAttribute.DoConserveMomentum, false);

    }
    public bool CheckIfCanAttack()
    {
        if (last_melee_attack_time == 0) // 時間 = 0 代表敵人初次觸發攻擊
            return true;
        else
            return Time.time >= last_melee_attack_time + enemyAttribute.MeleeCooldown;
    }

}
