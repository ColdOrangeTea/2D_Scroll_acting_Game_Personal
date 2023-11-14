using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoading : MonoBehaviour
{
    [Header("Parameter")]
    public static string SceneToLoad;
    private AsyncOperation async;
    int progress = 0;
    int Progress
    {
        get { return progress; }
        set
        {
            progress = value;
            if (progress >= 100) { sceneFade.FadeOut(); Debug.Log("轉場"); }
        }
    }
    [Header("Component")]
    [SerializeField] private SceneFade sceneFade;
    [Space(5)]

    [Header("Screen")]
    [SerializeField] private GameObject loading_screen; // 自行拖入
    //private string loading_screen_name = "LoadingSceneCanvas";
    [Space(5)]

    [Header("Slider")]
    [SerializeField] private Slider loading_slider; // 自行拖入
    //private string loading_slider_name = "Loading Slider";

    void Start()
    {
        sceneFade = GameObject.Find("FadeCanvas").GetComponent<SceneFade>();

        AddDelegatedEvent();
        sceneFade.FadeIn();
    }
    void AddDelegatedEvent()
    {
        AddSendFadeOut();
        AddSendFadeIn();
    }

    void AddSendFadeIn() => SendFading.SendSceneFadIn += OnSceneFadeIn;
    void CancelSendFadeIn() => SendFading.SendSceneFadIn -= OnSceneFadeIn;

    void AddSendFadeOut() => SendFading.SendSceneFadOut += OnSceneFadeOut;
    void CancelSendFadeOut() => SendFading.SendSceneFadOut -= OnSceneFadeOut;
    public void TestButton() //test
    {
        SceneToLoad = SceneOrder.Scene.ThingTest.ToString();
        StartCoroutine(LoadScene());
    }

    void OnSceneFadeIn(object sender, SceneFadeEventArgs e)
    {
        Debug.Log("SceneLoading " + "OnSceneFadeIn " + SceneLoading.SceneToLoad);
        StartCoroutine(LoadScene());
        CancelSendFadeIn();
    }

    void OnSceneFadeOut(object sender, SceneFadeEventArgs e)
    {
        Debug.Log("SceneLoading " + "OnSceneFadeOut " + SceneChangeManager.CurrentScene.name);
        OnFadeOutComplete();
        if (SceneChangeManager.CurrentScene.name == SceneOrder.Scene.ChangeScene.ToString())
            sceneFade.FadeIn();
    }

    public void OnFadeOutComplete()
    {
        async.allowSceneActivation = true;
    }

    IEnumerator LoadScene()
    {
        Progress = 0;
        int toProgress = 0;
        async = SceneManager.LoadSceneAsync(SceneToLoad);
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            toProgress = (int)async.progress * 100;
            while (Progress < toProgress)
            {
                ++Progress;
                SetLoadingSlider(Progress);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }
        toProgress = 100;
        while (Progress < toProgress)
        {
            ++Progress;
            SetLoadingSlider(Progress);
            yield return new WaitForEndOfFrame();
        }

    }

    void SetLoadingSlider(int p_progress)
    {
        float tmp = (float)((float)p_progress / 100);
        loading_slider.value = tmp;
    }
}
