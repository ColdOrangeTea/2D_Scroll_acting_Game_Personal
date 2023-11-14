using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingMenu;
    private bool SettingOpen = false;

    [SerializeField] private SceneChangeManager manager; // 自行拖入
    [SerializeField] private string scene_to_load = "[SceneName]";
    [SerializeField] private SceneOrder.Scene scene_to_load_name; // 自行設定
    private void Start()
    {
        manager = GameObject.Find("SceneChangeManager").GetComponent<SceneChangeManager>();
        scene_to_load = scene_to_load_name.ToString();

        SoundManager.Instance.getSliders();
        settingMenu.SetActive(false);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SettingOpen == true)
            {
                CloseSetting();
            }
        }
    }
    public void StartGame()
    {
        manager.GetSceneToLoad(scene_to_load);
        // SceneManager.LoadScene(SceneOrder.Scene.Level01.ToString());
    }
    public void LoadGame()//先進存檔選單但沒有讀檔功能
    {

    }
    public void OpenSetting()
    {
        settingMenu.SetActive(true);
        SettingOpen = true;
        SoundManager.Instance.getSliders();
    }
    public void CloseSetting()
    {
        settingMenu.SetActive(false);
        SettingOpen = false;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
