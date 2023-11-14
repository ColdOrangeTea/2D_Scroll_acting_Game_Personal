using System;
using UnityEngine;

public class SceneFade : MonoBehaviour
{
    public static SceneFade Instance;

    [Header("Parameter")]
    public static bool FadeOutIsDone;
    [Space(5)]

    [Header("Component")]
    [SerializeField] private Animator animator; // 自行拖入   
    [Space(5)]

    [Header("Delegate")]
    private SendFading sendFading;

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

    public void OnFadeOutComplete() // Animator Event
    {
        sendFading = new SendFading();
        sendFading.SendSceneFadeOut();
        animator.SetBool("FadeOut", false);
    }
    public void OnFadeInComplete() // Animator Event
    {
        sendFading = new SendFading();
        sendFading.SendSceneFadeIn();
        animator.SetBool("FadeIn", false);
    }
    public void Transparent()
    {
        Debug.Log("Transparent");
        animator.SetBool("FadeOut", false);
        animator.SetBool("FadeIn", false);
    }
    public void FadeOut()
    {
        Debug.Log("FadeOut");
        animator.SetBool("FadeOut", true);
        animator.SetBool("FadeIn", false);
    }

    public void FadeIn()
    {
        Debug.Log("FadeIn");
        animator.SetBool("FadeOut", false);
        animator.SetBool("FadeIn", true);

    }

}
