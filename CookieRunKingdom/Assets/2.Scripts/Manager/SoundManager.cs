using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct ClipInfo
{
    public string key;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private List<ClipInfo> _clipInfos = new List<ClipInfo>();


    private AudioSource _audioSource;
    private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        Instance = this;

        foreach(ClipInfo clipInfo in _clipInfos)
        {
            clips.Add(clipInfo.key, clipInfo.clip);
        }

        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayBG(string key)
    {
        _audioSource.clip = clips[key];
        _audioSource.Play();        
    }

    public void PlayFX(string key)
    {
        _audioSource.PlayOneShot(clips[key]);
    }
}
