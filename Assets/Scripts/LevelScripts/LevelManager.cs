using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public static int NumOfGears = 0;
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
