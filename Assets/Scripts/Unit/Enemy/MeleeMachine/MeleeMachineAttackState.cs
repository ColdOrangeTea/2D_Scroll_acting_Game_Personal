using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeMachineAttackState : AttackState
{
    public override IEnumerator Attackperiod()
    {
        yield return new WaitForSeconds(startUp);
        Debug.Log("Attackperiod");
        attackTransform.gameObject.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        Debug.Log("Attackperiod End");
        attackTransform.gameObject.SetActive(false);
    }

    // public override bool PlayerCheck(float moveDirection)
    // {
    //     if (Physics2D.Raycast(playerCheckTransform.position, new Vector3(moveDirection, 0), playerCheckX, playerLayer)
    //            || Physics2D.Raycast(playerCheckTransform.position + new Vector3(0, playerCheckY), new Vector3(moveDirection, 0), playerCheckX, playerLayer)
    //            || Physics2D.Raycast(playerCheckTransform.position + new Vector3(0, -playerCheckY), new Vector3(moveDirection, 0), playerCheckX, playerLayer))
    //     {
    //         // Debug.Log("有人在前面");
    //         return true;
    //     }
    //     else
    //     {
    //         // Debug.Log("沒人");
    //         return false;
    //     }
    // }

    public override IEnumerator Attacking()
    {
        Debug.Log("attacking");

        // Debug.Log("During attacking...");
        StartCoroutine(Attackperiod());

        yield return new WaitForSeconds(attackColdDown);
        unitStateMachineManager.SwitchStatus(UnitStateMachineManager.idle);
        // yield return new WaitForSeconds(attackColdDown);

        Debug.Log("attacking End");

        // if (!PlayerCheck(unitStateMachineManager.xAxis))
        // {
        //     unitStateMachineManager.SwitchStatus(UnitStateMachineManager.idle);
        //     // attackTransform.gameObject.SetActive(false);
        //     Debug.Log("attacking End");
        // }
        // else
        // {
        //     StartCoroutine(Attacking());
        //     Debug.Log("attacking End and continue attack");

        // }

    }
    public override void InitAttackState()
    {
        unitStateMachineManager = GetComponent<UnitStateMachineManager>();
        // playerCheckTransform = transform.GetChild(1).GetChild(3).GetComponent<Transform>();
        attackTransform = transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Transform>();
        startUp = 0.4f;
        attackDuration = 0.7f;
        attackColdDown = 1f;
        // playerCheckX = 2;
        // playerCheckY = 0.4f;
    }
}
