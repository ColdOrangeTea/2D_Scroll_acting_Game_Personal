using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectSlider;
    [SerializeField] private AudioSource _musicSource, _effectSource;

    [SerializeField] private const string musicMixer = "musicVolume";
    [SerializeField] private const string SFXMixer = "SFXVolume";

    //保留Soundmanager不讓BGM重來,音量已被PlayerPref保存了
    void Awake()
    {
        if (GameObject.Find("MusicVolume Slider") == null)
        {
            Debug.Log("No Slider");
        }
        else
        {
            getSliders();
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        CheckthenLoad();
    }
    private void Start()
    {
        CheckthenLoad();
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        _effectSource.PlayOneShot(clip);
    }

    /// <summary>
    /// audioMixer setvalue and save on 
    /// </summary>

    public void ChangeMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat(musicMixer, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(musicMixer, volume);
    }

    public void ChangeSoundVolume()
    {
        float volume = effectSlider.value;
        myMixer.SetFloat(SFXMixer, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFXMixer, volume);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat(musicMixer);
        effectSlider.value = PlayerPrefs.GetFloat(SFXMixer);
        ChangeMusicVolume();
        ChangeSoundVolume();
    }

    public void CheckthenLoad()
    {
        if (PlayerPrefs.HasKey(musicMixer))
        {
            LoadVolume();
        }
        else
        {
            ChangeMusicVolume();
            ChangeSoundVolume();
        }
    }

    public void getSliders()
    {
        musicSlider = GameObject.Find("MusicVolume Slider").GetComponent<Slider>();
        musicSlider.onValueChanged.AddListener(delegate { ChangeMusicVolume(); });
        effectSlider = GameObject.Find("EffectVolume Slider").GetComponent<Slider>();
        effectSlider.onValueChanged.AddListener(delegate { ChangeSoundVolume(); });
    }
}
