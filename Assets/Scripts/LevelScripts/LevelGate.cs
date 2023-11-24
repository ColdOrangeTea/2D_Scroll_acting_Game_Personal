using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        int goalNum;
        goalNum = LevelManager.Instance.GetGoalNum();
        if (SceneChangeManager.CurrentScene.name.ToString() == SceneOrder.Scene.Level01.ToString())
        {
            if (LevelManager.NumOfGears == goalNum)
            {
                gameObject.SetActive(false);
            }
        }
        else if (SceneChangeManager.CurrentScene.name.ToString() == SceneOrder.Scene.Level02.ToString())
        {
            if (LevelManager.NumOfGears == goalNum)
            {
                gameObject.SetActive(false);
            }
        }
        else if (SceneChangeManager.CurrentScene.name.ToString() == SceneOrder.Scene.Level03.ToString())
        {
            if (LevelManager.NumOfGears == goalNum)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
