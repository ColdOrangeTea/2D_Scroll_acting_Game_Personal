using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "ThingAttribute", menuName = "SO/DataTool/ Create ThingAttribute Asset", order = 3)]
public class ThingAttribute : ScriptableObject
{
    public string ThingName;

    [Header("Breakable Thing 掉落補血物等物品的物件")]
    public bool CanBeDamaged; // if it can't be damaged then it is an Interactive Object.

    public int BreakableThingID;

    [Header("Interactive Thing 觸發類的物件")]
    public bool CanBeOperated;

    public int InteractiveThingID;
    public float EffectedDuration;
    public float ActivatedCoolDown;
    // public float paralyzedRadius;

    [Header("Portable Thing 掉落補血物等物品的物件")]
    public bool CanBePickedUp;
    public int PortableThingID;
    public int DropChance;
    public bool IsHeal;
    public int HealAmount;
    public bool IsFunctional;
    public float ThingInvulnerableDuration;

    public Sprite ThingSprite;

    private void OnValidate()
    {

    }
}
