using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ThingAttribute))]
public class ThingAttributeInspector : Editor
{

    private ThingAttribute attribute;

    #region  BOOL 
    private bool e_is_show_up_CanBeDamaged;
    private bool e_is_show_up_CanBeOperated;
    private bool e_is_show_up_CanBePickedUp;
    #endregion

    public void Awake()
    {
        attribute = (ThingAttribute)target;
        if (attribute.CanBeDamaged)
        {
            e_is_show_up_CanBeDamaged = true;
        }
        if (attribute.CanBeOperated)
        {
            e_is_show_up_CanBeOperated = true;
        }
        if (attribute.CanBePickedUp)
        {
            e_is_show_up_CanBePickedUp = true;
        }
    }
    public override void OnInspectorGUI()
    {
        // DrawDefaultInspector();
        #region  NAME
        attribute.ThingName = attribute.ThingName = EditorGUILayout.TextField("物品的名字", attribute.ThingName);
        #endregion

        #region  BREAKABLE
        attribute.CanBeDamaged = EditorGUILayout.Toggle("是不是可破壞物件", attribute.CanBeDamaged);
        e_is_show_up_CanBeDamaged = EditorGUILayout.BeginFoldoutHeaderGroup(e_is_show_up_CanBeDamaged, "可破壞物的物件資料");
        EditorGUI.BeginDisabledGroup(attribute.CanBeDamaged == false);

        if (e_is_show_up_CanBeDamaged)
        {
            attribute.BreakableThingID = EditorGUILayout.IntField("BreakableThingID: ", attribute.BreakableThingID);
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUI.EndDisabledGroup();
        #endregion

        #region  INTERACTIVE
        attribute.CanBeOperated = EditorGUILayout.Toggle("是不是可啟動的裝置物件", attribute.CanBeOperated);
        e_is_show_up_CanBeOperated = EditorGUILayout.BeginFoldoutHeaderGroup(e_is_show_up_CanBeOperated, "可啟動物的物件資料");

        EditorGUI.BeginDisabledGroup(attribute.CanBeOperated == false);
        if (e_is_show_up_CanBeOperated)
        {
            attribute.InteractiveThingID = EditorGUILayout.IntField("InteractiveThingID: ", attribute.InteractiveThingID);
            attribute.EffectedDuration = EditorGUILayout.FloatField("EffectedDuration: ", attribute.EffectedDuration);
            attribute.ActivatedCoolDown = EditorGUILayout.FloatField("ActivatedCoolDown: ", attribute.ActivatedCoolDown);
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUI.EndDisabledGroup();
        #endregion

        #region  PORTABLE
        attribute.CanBePickedUp = EditorGUILayout.Toggle("是不是可撿取物", attribute.CanBePickedUp);
        e_is_show_up_CanBePickedUp = EditorGUILayout.BeginFoldoutHeaderGroup(e_is_show_up_CanBePickedUp, "可撿取物的物件資料");

        EditorGUI.BeginDisabledGroup(attribute.CanBePickedUp == false);
        if (e_is_show_up_CanBePickedUp)
        {
            attribute.PortableThingID = EditorGUILayout.IntField("PortableThingID: ", attribute.PortableThingID);
            attribute.DropChance = EditorGUILayout.IntField("DropChance", attribute.DropChance);
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUI.EndDisabledGroup();
        #endregion
    }

}


