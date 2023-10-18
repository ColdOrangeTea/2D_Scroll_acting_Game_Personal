using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public const string TitleMenu = "TitleMenu";

    public static PauseMenu Instance;
    public static bool GameIsPaused = false;
    public bool SettingOpening;

    public GameObject PauseMenuUI;
    public GameObject SettingMenu;
    private void Start()
    {
        //SoundManager.Instance.getSliders();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PauseMenuUI.SetActive(false);
            SettingMenu.SetActive(false);

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
                if (SceneManager.GetActiveScene().name != TitleMenu)
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
        SettingMenu.SetActive(true);
        SettingOpening = true;
        SoundManager.Instance.getSliders();
    }
    public void CloseSetting()
    {
        SettingMenu.SetActive(false);
        SettingOpening = false;
    }
    public void BackToTitle()
    {
        Resume();
        CloseSetting();
        SceneManager.LoadScene(TitleMenu);
        Debug.Log("scenesLoaded");

    }
}
