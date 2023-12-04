using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private GameObject PlayerUICanva;
    [SerializeField] private int Level01_GoalNum = 6;
    [SerializeField] private int Level02_GoalNum = 6;
    [SerializeField] private int Level03_GoalNum = 6;
    public bool L1Fin = false;
    public bool L2Fin = false;
    public bool L3Fin = false;

    public static int NumOfGears = 0;
    public Text GearCount;
    public Text ToNextLevel_00;
    public Text ToNextLevel_01;
    public Image hpBar;
    public int UIHp;

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
        if (SceneChangeManager.CurrentScene.name.ToString() == SceneOrder.Scene.TitleMenu.ToString())
        {
            PlayerUICanva.SetActive(false);
            GearCount.gameObject.SetActive(false);
            ToNextLevel_00.gameObject.SetActive(false);
            ToNextLevel_01.gameObject.SetActive(false);
        }
        if (SceneChangeManager.CurrentScene.name.ToString() == SceneOrder.Scene.Level01.ToString())
        {
            if (NumOfGears == Level01_GoalNum && !L1Fin)
            {
                L1Fin = true;
                AudioManager.Instance.PlaySound(AudioType.tags.Level_Success, this.gameObject.transform);
            }
            if (!PlayerUICanva.activeInHierarchy)
                PlayerUICanva.SetActive(true);
            if (!GearCount.gameObject.activeInHierarchy)
                GearCount.gameObject.SetActive(true);

            GearCount.text = "             " + NumOfGears;
            if (L1Fin)
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
            if (NumOfGears == Level02_GoalNum && !L2Fin)
            {
                L2Fin = true;
                AudioManager.Instance.PlaySound(AudioType.tags.Level_Success, this.gameObject.transform);
            }
            if (!PlayerUICanva.activeInHierarchy)
                PlayerUICanva.SetActive(true);
            if (!GearCount.gameObject.activeInHierarchy)
                GearCount.gameObject.SetActive(true);

            GearCount.text = "             " + NumOfGears;
            if (L2Fin)
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
            if (NumOfGears == Level03_GoalNum && !L3Fin)
            {
                L3Fin = true;
                AudioManager.Instance.PlaySound(AudioType.tags.Level_Success, this.gameObject.transform);
            }
            if (!PlayerUICanva.activeInHierarchy)
                PlayerUICanva.SetActive(true);
            if (!GearCount.gameObject.activeInHierarchy)
                GearCount.gameObject.SetActive(true);

            GearCount.text = "             " + NumOfGears;
            if (L3Fin)
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
    public void UpdateUIHpBar(int MaxHp, int curHp)
    {
        Debug.Log("血量 " + curHp + " / " + MaxHp);
        hpBar.fillAmount = (0.01f * curHp) / (0.01f * MaxHp);
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
