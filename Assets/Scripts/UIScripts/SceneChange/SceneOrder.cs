using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOrder
{
    public enum Scene
    {
        TitleMenu = 0,
        Level01 = 1,
        Level02 = 2,
        Level03 = 3,
        ChangeScene = 10,
        ThingTest = 11,
        TestScene2 = 12
    }
    // void Start()
    // {
    //     string n = Scene.Level01.ToString();
    //     Debug.Log("Active Scene is '" + n + "'.");
    // }
}
