using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectSlider;
    [Space(5)]
    [Header("Volume")]
    [Range(0.0001f, 1)]
    [SerializeField] private float BGMVolume;

    [Range(0.0001f, 1)]
    [SerializeField] private float SoundVolume;

    [Space(5)]
    [Header("BGM List")]
    public List<AudioClip> musicList;
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
        BGMVolume = musicSlider.value;
        myMixer.SetFloat(musicMixer, Mathf.Log10(BGMVolume) * 20);
        PlayerPrefs.SetFloat(musicMixer, BGMVolume);
    }

    public void ChangeSoundVolume()
    {
        SoundVolume = effectSlider.value;
        myMixer.SetFloat(SFXMixer, Mathf.Log10(SoundVolume) * 20);
        PlayerPrefs.SetFloat(SFXMixer, SoundVolume);
    }

    public void LoadVolume()
    {
        // for test
        musicSlider.value = BGMVolume;
        effectSlider.value = SoundVolume;

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
