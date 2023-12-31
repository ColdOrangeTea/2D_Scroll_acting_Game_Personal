using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingMenu;
    [SerializeField] private GameObject titleMenu;

    private bool SettingIsOpening = false;

    [SerializeField] private SceneChangeManager manager; // 自行拖入
    [SerializeField] private string scene_to_load = "[SceneName]";
    [SerializeField] private SceneOrder.Scene scene_to_load_name; // 自行設定

    private void Start()
    {
        manager = GameObject.Find("SceneChangeManager").GetComponent<SceneChangeManager>();
        scene_to_load = scene_to_load_name.ToString();

        AudioManager.Instance.getSliders();
        settingMenu.SetActive(false);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SettingIsOpening == true)
            {
                CloseSetting();
            }
        }
    }

    // IEnumerator FadingOutMenuPanel()
    // {
    //     var menuColor = titleMenu.GetComponent<Image>().color;
    //     var desiredColor = new Color(menuColor.r, menuColor.g, menuColor.b, 0);
    //     // titleMenu.GetComponent<Image>().color = Color.Lerp(menuColor, desiredColor, 0.1f);
    //     titleMenu.GetComponent<Image>().color = desiredColor;

    //     Debug.Log("UI透明度: " + titleMenu.GetComponent<Image>().color);
    //     yield return new WaitForEndOfFrame();
    // }
    public void StartGame()
    {
        if (SettingIsOpening) return;
        titleMenu.SetActive(false);
        // StartCoroutine(FadingOutMenuPanel());

        manager.GetSceneToLoad(scene_to_load);
    }
    public void LoadGame()//先進存檔選單但沒有讀檔功能
    {

    }
    public void OpenSetting()
    {
        if (SettingIsOpening) return;

        settingMenu.SetActive(true);
        SettingIsOpening = true;
        AudioManager.Instance.getSliders();
    }
    public void CloseSetting()
    {
        settingMenu.SetActive(false);
        SettingIsOpening = false;
    }

    public void QuitGame()
    {
        if (SettingIsOpening) return;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
