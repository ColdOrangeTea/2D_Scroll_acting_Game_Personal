using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMachineDeadState : DeadState
{

    public override IEnumerator Dying()
    {
        thisThing.gameObject.SetActive(false);
        Destroy(thisThing);
        yield return null;
    }
    public override void InitDeadState()
    {
        unitStateMachineManager = GetComponent<UnitStateMachineManager>();
        thisThing = this.gameObject;
    }
}
