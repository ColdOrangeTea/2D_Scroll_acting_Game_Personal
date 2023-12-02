using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public static bool GameIsPaused = false;

    public bool SettingIsOpening;

    public GameObject PauseMenuUI; // 自行拖入
    public GameObject SettingMenuUI;
    public GameObject CheatMenuUI;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PauseMenuUI.SetActive(false);
            SettingMenuUI.SetActive(false);
            CheatMenuUI.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (CheatMenuUI.activeInHierarchy)
            {
                CheatMenuUI.SetActive(false);
            }
            else
            {
                if (SceneManager.GetActiveScene().name != SceneOrder.Scene.TitleMenu.ToString())
                    CheatMenuUI.SetActive(true);
            }

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (GameIsPaused)
            {

                if (!SettingIsOpening)
                    Resume();
            }
            else
            {
                if (SceneManager.GetActiveScene().name != SceneOrder.Scene.TitleMenu.ToString())
                    Pause();
            }
        }
    }

    public void Resume()
    {
        if (SettingIsOpening) return;
        PauseMenuUI.SetActive(false);
        CheatMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void OpenSetting()
    {
        SettingMenuUI.SetActive(true);
        SettingIsOpening = true;
        AudioManager.Instance.getSliders();
    }
    public void CloseSetting()
    {
        SettingMenuUI.SetActive(false);
        SettingIsOpening = false;
    }
    public void BackToTitle()
    {
        if (SettingIsOpening) return;
        Resume();
        CloseSetting();
        SceneManager.LoadScene(SceneOrder.Scene.TitleMenu.ToString());
        Debug.Log("scenesLoaded");

    }
}
