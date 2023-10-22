using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager Instance;

    [Header("CurrentScene")]
    [Space(5)]
    [SerializeField] private Scene CurrentScene;
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
        GetCurrentScene();
        Debug.Log("Active Scene is '" + cur_room_name + "' " + ".");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        // Debug.Log("OnSceneLoaded: " + scene.name);
        // Debug.Log(loadSceneMode);
        GetCurrentScene();
    }

    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void GetCurrentScene()
    {
        CurrentScene = SceneManager.GetActiveScene();
        cur_room_name = CurrentScene.name;
    }

}
