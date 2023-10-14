using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ThingAttribute", menuName = "DataTool/ Create ThingAttribute Asset", order = 3)]
public class ThingAttribute : ScriptableObject
{
    public string ObjectName;
    public bool CanBeDamaged; // if it can't be damaged then it is an Interactive Object.
    public bool CanBePickUp;
    [Header("Box 掉落補血物等物品的物件")]

    // [Header("Artillery 定點攻擊類的物件")]
    // public bool IsArtillery;

    [Header("Interactive Thing 觸發類的物件")]
    public float EffectedDuration;
    public float ActivatedCoolDown;
    // public float paralyzedRadius;


    private void OnValidate()
    {
        if (CanBePickUp)
        {
            CanBeDamaged = false;
        }
    }
}
