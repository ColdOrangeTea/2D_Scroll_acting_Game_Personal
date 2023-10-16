using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ThingAttribute))]
public class ThingAttributeInspector : Editor
{

    private ThingAttribute attribute;

    #region  BOOL 
    private bool e_is_breakable_thing;
    private bool e_is_interactive_thing;
    private bool e_is_portable_thing;
    #endregion

    private string e_thing_name;

    public void Awake()
    {
        attribute = (ThingAttribute)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        #region  NAME
        e_thing_name = attribute.ThingName = EditorGUILayout.TextField("物品的名字", attribute.ThingName);
        attribute.ThingName = e_thing_name;
        #endregion

        #region  BREAKABLE
        e_is_breakable_thing = EditorGUILayout.Toggle("is_breakable_thing", attribute.CanBeDamaged);
        attribute.CanBeDamaged = e_is_breakable_thing;
        if (e_is_breakable_thing)
        {
            e_is_interactive_thing = false;
            e_is_portable_thing = false;
        }
        EditorGUI.BeginDisabledGroup(e_is_breakable_thing == false);

        attribute.BreakableThingID = EditorGUILayout.IntField("ID: ", attribute.BreakableThingID);

        EditorGUI.EndDisabledGroup();
        #endregion

        #region  INTERACTIVE
        e_is_interactive_thing = EditorGUILayout.Toggle("is_interactive_thing", attribute.CanBeOperated);
        attribute.CanBeOperated = e_is_interactive_thing;
        if (e_is_interactive_thing)
        {
            e_is_breakable_thing = false;
            e_is_portable_thing = false;
        }
        EditorGUI.BeginDisabledGroup(e_is_interactive_thing == false);

        attribute.InteractiveThingID = EditorGUILayout.IntField("ID: ", attribute.InteractiveThingID);
        attribute.EffectedDuration = EditorGUILayout.FloatField("EffectedDuration: ", attribute.InteractiveThingID);
        attribute.ActivatedCoolDown = EditorGUILayout.FloatField("ActivatedCoolDown: ", attribute.InteractiveThingID);

        EditorGUI.EndDisabledGroup();
        #endregion

        #region  PORTABLE
        e_is_portable_thing = EditorGUILayout.Toggle("is_portable_thing", attribute.CanBePickedUp);
        attribute.CanBePickedUp = e_is_portable_thing;
        if (e_is_portable_thing)
        {
            e_is_breakable_thing = false;
            e_is_interactive_thing = false;
        }
        EditorGUI.BeginDisabledGroup(e_is_portable_thing == false);

        attribute.PortableThingID = EditorGUILayout.IntField("ID: ", attribute.InteractiveThingID);

        EditorGUI.EndDisabledGroup();
        #endregion

    }

    // private void OnValidate()
    // {
    //     if (e_is_breakable_thing)
    //     {
    //         e_is_interactive_thing = false;
    //         e_is_portable_thing = false;
    //     }
    //     if (e_is_interactive_thing)
    //     {
    //         e_is_breakable_thing = false;
    //         e_is_portable_thing = false;
    //     }
    //     if (e_is_portable_thing)
    //     {
    //         e_is_breakable_thing = false;
    //         e_is_interactive_thing = false;
    //         // attribute.CanBeDamaged = false;
    //         // attribute.CanBeOperated = false;
    //         // attribute.EffectedDuration = 0;
    //         // attribute.ActivatedCoolDown = 0;
    //     }

    // }
}
