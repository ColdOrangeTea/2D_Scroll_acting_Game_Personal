using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private int Level01_GoalNum = 6;
    [SerializeField] private int Level02_GoalNum = 6;
    [SerializeField] private int Level03_GoalNum = 6;

    public static int NumOfGears = 0;
    public Text GearCount;
    public Text ToNextLevel_00;
    public Text ToNextLevel_01;

    // [SerializeField] private bool IsGameOver = false;
    // public static bool IsGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        GearCount.gameObject.SetActive(true);
        ToNextLevel_00.gameObject.SetActive(false);
        ToNextLevel_01.gameObject.SetActive(false);
    }
    private void Update()
    {

        if (SceneChangeManager.CurrentScene.name.ToString() == SceneOrder.Scene.Level01.ToString())
        {
            GearCount.text = "Gear: " + NumOfGears + " / " + Level01_GoalNum;
            if (NumOfGears == Level01_GoalNum)
            {
                GearCount.color = Color.green;
                ToNextLevel_00.gameObject.SetActive(true);
                ToNextLevel_01.gameObject.SetActive(true);
            }
            else
            {
                GearCount.color = Color.white;
                GearCount.gameObject.SetActive(true);
                ToNextLevel_00.gameObject.SetActive(false);
                ToNextLevel_01.gameObject.SetActive(false);
            }
        }
        else if (SceneChangeManager.CurrentScene.name.ToString() == SceneOrder.Scene.Level02.ToString())
        {
            GearCount.text = "Gear: " + NumOfGears + " / " + Level02_GoalNum;
            if (NumOfGears == Level02_GoalNum)
            {
                GearCount.color = Color.green;
                ToNextLevel_00.gameObject.SetActive(true);
                ToNextLevel_01.gameObject.SetActive(true);
            }
            else
            {
                GearCount.color = Color.white;
                GearCount.gameObject.SetActive(true);
                ToNextLevel_00.gameObject.SetActive(false);
                ToNextLevel_01.gameObject.SetActive(false);
            }
        }
        else if (SceneChangeManager.CurrentScene.name.ToString() == SceneOrder.Scene.Level03.ToString())
        {
            GearCount.text = "Gear: " + NumOfGears + " / " + Level03_GoalNum;
            if (NumOfGears == Level03_GoalNum)
            {
                GearCount.color = Color.green;
                ToNextLevel_00.gameObject.SetActive(true);
                ToNextLevel_01.gameObject.SetActive(true);
            }
            else
            {
                GearCount.color = Color.white;
                GearCount.gameObject.SetActive(true);
                ToNextLevel_00.gameObject.SetActive(false);
                ToNextLevel_01.gameObject.SetActive(false);
            }
        }

    }
    public int GetGoalNum()
    {
        int goalNum;
        if (SceneChangeManager.CurrentScene.name.ToString() == SceneOrder.Scene.Level01.ToString())
        {
            goalNum = Level01_GoalNum;
            return goalNum;
        }
        else if (SceneChangeManager.CurrentScene.name.ToString() == SceneOrder.Scene.Level02.ToString())
        {
            goalNum = Level02_GoalNum;

            return goalNum;
        }
        else if (SceneChangeManager.CurrentScene.name.ToString() == SceneOrder.Scene.Level03.ToString())
        {
            goalNum = Level03_GoalNum;
            return goalNum;
        }
        else
        {
            return 0;
        }

    }
    //private void Update()
    //{
    //    if (!IsGameOver) return;
    //}
    //public void GameOver()
    //{
    //    IsGameOver=true;
    //}

}
