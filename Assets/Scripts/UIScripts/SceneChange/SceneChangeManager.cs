using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager Instance;
    private AsyncOperation async;
    [SerializeField] private SceneFade sceneFade;

    [Header("CurrentScene")]
    [Space(5)]
    [SerializeField] public static Scene CurrentScene;
    [SerializeField] private string cur_room_name = "[SceneName]";

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

    void Start()
    {
        sceneFade = GameObject.Find("FadeCanvas").GetComponent<SceneFade>();

        AddDelegatedEvent();
        GetCurrentScene();
        // Debug.Log("Active Scene is '" + cur_room_name + "' " + ".");
    }
    void AddDelegatedEvent()
    {
        AddSceneManager();
        // AddSendFadeIn();
        AddSendFadeOut();
    }

    #region DELEGATE METHOD 
    // void AddSendFadeIn() => SendFading.SendSceneFadIn += OnSceneFadeIn;
    // void CancelSendFadeIn() => SendFading.SendSceneFadIn -= OnSceneFadeIn;

    void AddSendFadeOut() => SendFading.SendSceneFadOut += OnSceneFadeOut;
    void CancelSendFadeOut() => SendFading.SendSceneFadOut -= OnSceneFadeOut;

    void AddSceneManager() => SceneManager.sceneLoaded += OnSceneLoaded;
    void CancelSceneManager() => SceneManager.sceneLoaded -= OnSceneLoaded;

    // void OnSceneFadeIn(object sender, SceneFadeEventArgs e)
    // {
    //     Debug.Log("SceneChangeManager " + "OnSceneFadeIn " + SceneLoading.SceneToLoad);
    //     CancelSendFadeIn();
    // }
    void OnSceneFadeOut(object sender, SceneFadeEventArgs e)
    {
        Debug.Log("SceneChangeManager " + "OnSceneFadeOut " + CurrentScene.name + SceneLoading.SceneToLoad);
        if (CurrentScene.name != SceneOrder.Scene.ChangeScene.ToString())
            SceneManager.LoadSceneAsync(SceneOrder.Scene.ChangeScene.ToString());
        // SceneManager.LoadSceneAsync(SceneOrder.Scene.Level01.ToString());
        //CancelSendFadeOut();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("SceneChangeManager " + "OnSceneLoaded: " + scene.name);
        // Debug.Log(loadSceneMode);
        GetCurrentScene();
    }
    #endregion



    public void GetSceneToLoad(string sceneToLoad)
    {
        SceneLoading.SceneToLoad = sceneToLoad;
        sceneFade.FadeOut();
    }

    public void GetCurrentScene()
    {
        CurrentScene = SceneManager.GetActiveScene();
        cur_room_name = CurrentScene.name;
    }

}
