using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "Loot", menuName = "SO/DataTool/ Create Loot Asset", order = 5)]
public class Loot : ScriptableObject
{
    [Header("Loot 戰利品 ")]
    public ThingAttribute attribute;
    // [Space(10)]
    // public Sprite ThingSprite;
    // public int DropChance;
    // private void OnValidate()
    // {
    //     if (attribute != null)
    //     {
    //         ThingSprite = attribute.ThingSprite;
    //         DropChance = attribute.DropChance;
    //     }
    // }
}
