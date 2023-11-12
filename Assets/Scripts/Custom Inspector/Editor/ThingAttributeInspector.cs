using UnityEngine;
using UnityEditor;
using System.Collections;
// using UnityEngine.UIElements;
// using UnityEditor.UIElements;

[CustomEditor(typeof(ThingAttribute))]
public class ThingAttributeInspector : Editor
{
    private ThingAttribute attribute;

    #region  STRING 
    private string thingName;
    #endregion

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

    // public override VisualElement CreateInspectorGUI()
    // {
    //     // Create a new VisualElement to be the root of our inspector UI
    //     VisualElement myInspector = new VisualElement();

    //     // Add a simple label
    //     myInspector.Add(new Label("This is a custom inspector"));

    //     // Return the finished inspector UI
    //     return myInspector;
    // }

    public override void OnInspectorGUI()
    {
        attribute = (ThingAttribute)target;
        // DrawDefaultInspector();

        GUIStyle style = new GUIStyle();
        style.richText = true; // 允許（Rich Text Format），RTF格式啟用
        EditorGUILayout.LabelField("<size=16><color=white><b>只能勾選其中一個!!</b> </color> </size> ", style);
        EditorGUILayout.Space(10);

        #region  NAME
        thingName = EditorGUILayout.TextField("物品的名字", attribute.ThingName); // 顯示字串，預設值
        attribute.ThingName = thingName;
        // attribute.ThingName = EditorGUILayout.TextField("物品的名字", attribute.ThingName);
        #endregion

        #region  BREAKABLE
        e_is_show_up_CanBeDamaged = EditorGUILayout.Toggle("是不是可破壞物件", attribute.CanBeDamaged);
        attribute.CanBeDamaged = e_is_show_up_CanBeDamaged;

        e_is_show_up_CanBeDamaged = EditorGUILayout.BeginFoldoutHeaderGroup(e_is_show_up_CanBeDamaged, "可破壞物的物件資料");
        EditorGUI.BeginDisabledGroup(e_is_show_up_CanBeDamaged == false);

        if (e_is_show_up_CanBeDamaged)
        {
            attribute.BreakableThingID = EditorGUILayout.IntField("BreakableThingID: ", attribute.BreakableThingID);
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        #region  INTERACTIVE
        e_is_show_up_CanBeOperated = EditorGUILayout.Toggle("是不是可啟動的裝置物件", attribute.CanBeOperated);
        attribute.CanBeOperated = e_is_show_up_CanBeOperated;

        e_is_show_up_CanBeOperated = EditorGUILayout.BeginFoldoutHeaderGroup(e_is_show_up_CanBeOperated, "可啟動物的物件資料");
        EditorGUI.BeginDisabledGroup(e_is_show_up_CanBeOperated == false);

        if (e_is_show_up_CanBeOperated)
        {
            attribute.InteractiveThingID = EditorGUILayout.IntField("InteractiveThingID: ", attribute.InteractiveThingID);
            attribute.EffectedDuration = EditorGUILayout.FloatField("EffectedDuration: ", attribute.EffectedDuration);
            attribute.ActivatedCoolDown = EditorGUILayout.FloatField("ActivatedCoolDown: ", attribute.ActivatedCoolDown);
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        #region  PORTABLE
        e_is_show_up_CanBePickedUp = EditorGUILayout.Toggle("是不是可撿取物", attribute.CanBePickedUp);
        attribute.CanBePickedUp = e_is_show_up_CanBePickedUp;

        e_is_show_up_CanBePickedUp = EditorGUILayout.BeginFoldoutHeaderGroup(e_is_show_up_CanBePickedUp, "可撿取物的物件資料");
        EditorGUI.BeginDisabledGroup(e_is_show_up_CanBePickedUp == false);
        if (e_is_show_up_CanBePickedUp)
        {
            attribute.PortableThingID = EditorGUILayout.IntField("PortableThingID: ", attribute.PortableThingID);
            attribute.DropChance = EditorGUILayout.IntField("DropChance", attribute.DropChance);

            attribute.ThingSprite = (Sprite)EditorGUILayout.ObjectField("圖片:", attribute.ThingSprite, typeof(Sprite), true);
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        EditorUtility.SetDirty(attribute);
    }


    // public override void OnInspectorGUI()
    // {
    //     DrawDefaultInspector();
    //     #region  NAME

    //     thingName = EditorGUILayout.TextField("物品的名字", attribute.ThingName); // 顯示字串，預設值
    //     attribute.ThingName = thingName;
    //     // attribute.ThingName = EditorGUILayout.TextField("物品的名字", attribute.ThingName);
    //     #endregion

    //     #region  BREAKABLE
    //     e_is_show_up_CanBeDamaged = EditorGUILayout.Toggle("是不是可破壞物件", attribute.CanBeDamaged);
    //     attribute.CanBeDamaged = e_is_show_up_CanBeDamaged;

    //     e_is_show_up_CanBeDamaged = EditorGUILayout.BeginFoldoutHeaderGroup(e_is_show_up_CanBeDamaged, "可破壞物的物件資料");
    //     EditorGUI.BeginDisabledGroup(attribute.CanBeDamaged == false);

    //     if (e_is_show_up_CanBeDamaged)
    //     {
    //         attribute.BreakableThingID = EditorGUILayout.IntField("BreakableThingID: ", attribute.BreakableThingID);
    //     }
    //     EditorGUI.EndDisabledGroup();
    //     EditorGUILayout.EndFoldoutHeaderGroup();
    //     #endregion

    //     #region  INTERACTIVE
    //     attribute.CanBeOperated = EditorGUILayout.Toggle("是不是可啟動的裝置物件", attribute.CanBeOperated);
    //     e_is_show_up_CanBeOperated = EditorGUILayout.BeginFoldoutHeaderGroup(e_is_show_up_CanBeOperated, "可啟動物的物件資料");

    //     EditorGUI.BeginDisabledGroup(attribute.CanBeOperated == false);
    //     if (e_is_show_up_CanBeOperated)
    //     {
    //         attribute.InteractiveThingID = EditorGUILayout.IntField("InteractiveThingID: ", attribute.InteractiveThingID);
    //         attribute.EffectedDuration = EditorGUILayout.FloatField("EffectedDuration: ", attribute.EffectedDuration);
    //         attribute.ActivatedCoolDown = EditorGUILayout.FloatField("ActivatedCoolDown: ", attribute.ActivatedCoolDown);
    //     }
    //     EditorGUI.EndDisabledGroup();
    //     EditorGUILayout.EndFoldoutHeaderGroup();

    //     #endregion

    //     #region  PORTABLE
    //     attribute.CanBePickedUp = EditorGUILayout.Toggle("是不是可撿取物", attribute.CanBePickedUp);
    //     e_is_show_up_CanBePickedUp = EditorGUILayout.BeginFoldoutHeaderGroup(e_is_show_up_CanBePickedUp, "可撿取物的物件資料");

    //     EditorGUI.BeginDisabledGroup(attribute.CanBePickedUp == false);
    //     if (e_is_show_up_CanBePickedUp)
    //     {
    //         attribute.PortableThingID = EditorGUILayout.IntField("PortableThingID: ", attribute.PortableThingID);
    //         attribute.DropChance = EditorGUILayout.IntField("DropChance", attribute.DropChance);

    //         attribute.ThingSprite = (Sprite)EditorGUILayout.ObjectField("圖片:", attribute.ThingSprite, typeof(Sprite), true);
    //     }
    //     EditorGUI.EndDisabledGroup();
    //     EditorGUILayout.EndFoldoutHeaderGroup();

    //     #endregion
    // }
}


