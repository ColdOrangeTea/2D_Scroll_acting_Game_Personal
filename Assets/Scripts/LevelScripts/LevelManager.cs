using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int Level01_GoalNum = 6;
    public static int NumOfGears = 0;
    public Text GearCount;
    public Text ToNextLevel_00;
    public Text ToNextLevel_01;
    public GameObject Level01Gate;

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
        GearCount.text = "Gear: " + NumOfGears + " / " + Level01_GoalNum;
        if (NumOfGears == Level01_GoalNum)
        {
            GearCount.color = Color.green;
            ToNextLevel_00.gameObject.SetActive(true);
            ToNextLevel_01.gameObject.SetActive(true);
            Level01Gate.SetActive(false);
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
