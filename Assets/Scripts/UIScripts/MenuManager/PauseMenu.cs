using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public static bool GameIsPaused = false;
    public bool SettingOpening;

    public GameObject PauseMenuUI; // 自行拖入
    public GameObject SettingMenuUI;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PauseMenuUI.SetActive(false);
            SettingMenuUI.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused && !SettingOpening)
            {
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
        PauseMenuUI.SetActive(false);
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
        SettingOpening = true;
        SoundManager.Instance.getSliders();
    }
    public void CloseSetting()
    {
        SettingMenuUI.SetActive(false);
        SettingOpening = false;
    }
    public void BackToTitle()
    {
        Resume();
        CloseSetting();
        SceneManager.LoadScene(SceneOrder.Scene.TitleMenu.ToString());
        Debug.Log("scenesLoaded");

    }
}
