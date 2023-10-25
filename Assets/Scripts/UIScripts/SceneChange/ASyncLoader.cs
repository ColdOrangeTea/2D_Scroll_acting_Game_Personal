using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class ASyncLoader : MonoBehaviour
{
    public static ASyncLoader Instance;

    [Header("Screen")]
    [SerializeField] private GameObject loading_screen; // 自行拖入
    private string loading_screen_name = "LoadingSceneCanvas";

    [Header("Slider")]
    [SerializeField] private Slider loading_slider;
    private string loading_slider_name = "Loading Slider";

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

    public void LoadLevelBtn(string levelToLoad)
    {
        // GetSlider();
        // GetScreen();
        loading_screen.SetActive(true);
        StartCoroutine(LoadLevelASync(levelToLoad));
    }

    IEnumerator LoadLevelASync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loading_slider.value = progressValue;
            yield return null;
        }
    }

    // public void GetSlider()
    // {
    //     loading_slider = GameObject.Find(loading_slider_name).GetComponent<Slider>();
    // }
    // public void GetScreen()
    // {
    //     loading_screen = GameObject.Find(loading_screen_name).GetComponent<GameObject>();

    // }
}
