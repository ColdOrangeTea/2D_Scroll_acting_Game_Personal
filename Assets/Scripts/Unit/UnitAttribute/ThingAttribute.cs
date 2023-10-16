using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "ThingAttribute", menuName = "DataTool/ Create ThingAttribute Asset", order = 3)]
public class ThingAttribute : ScriptableObject
{
    public string ThingName;

    [Header("Breakable Thing 掉落補血物等物品的物件")]
    public int BreakableThingID;
    public bool CanBeDamaged; // if it can't be damaged then it is an Interactive Object.
    // [Header("Artillery 定點攻擊類的物件")]
    // public bool IsArtillery;

    [Header("Interactive Thing 觸發類的物件")]
    public int InteractiveThingID;
    public bool CanBeOperated;
    public float EffectedDuration;
    public float ActivatedCoolDown;
    // public float paralyzedRadius;

    [Header("Portable Thing 掉落補血物等物品的物件")]
    public int PortableThingID;
    public bool CanBePickedUp;

    private void OnValidate()
    {
        if (CanBePickedUp)
        {
            CanBeDamaged = false;
            EffectedDuration = 0;
            ActivatedCoolDown = 0;
        }
        if (!CanBeDamaged && !CanBePickedUp)
        {
            CanBeOperated = true;
        }
    }
}
