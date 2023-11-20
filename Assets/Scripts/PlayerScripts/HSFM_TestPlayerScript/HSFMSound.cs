using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
[SerializeField]
public class MusicData
{
    public string BGMName;
    public AudioClip BGM;
}
public class HSFMSound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> soundList;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
