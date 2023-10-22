using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingMenu;
    private bool SettingOpen = false;
    private void Start()
    {
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
        SceneManager.LoadScene(SceneOrder.Scene.Level01.ToString());
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
