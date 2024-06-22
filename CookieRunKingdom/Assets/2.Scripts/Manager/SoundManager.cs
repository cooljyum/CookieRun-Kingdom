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
    [SerializeField]
    private List<ClipInfo> _clipEffectInfos = new List<ClipInfo>();

    private AudioSource _audioSource;

    private Dictionary<string, AudioClip> _clips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> _clipEffects = new Dictionary<string, AudioClip>();

    private float _bgVolume = 1f;
    private float _fxVolume = 1f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        foreach(ClipInfo clipInfo in _clipInfos)
        {
            _clips.Add(clipInfo.key, clipInfo.clip);
        }

        foreach (ClipInfo clipInfo in _clipEffectInfos)
        {
            _clipEffects.Add(clipInfo.key, clipInfo.clip);
        }

        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayBG(string key)
    {
        StopBG();

        if (_clips.TryGetValue(key, out AudioClip clip))
        {
            _audioSource.clip = clip;
            _audioSource.volume = _bgVolume;
            _audioSource.loop = true; // 배경음은 일반적으로 루프 재생
            _audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"BG clip with key {key} not found!");
        }
    }

    public void PlayFX(string key)
    {
        if (_clipEffects.TryGetValue(key, out AudioClip clip))
        {
            StartCoroutine(PlayFXCoroutine(clip));
        }
        else
        {
            Debug.LogWarning($"FX clip with key {key} not found!");
        }
    }

    private IEnumerator PlayFXCoroutine(AudioClip clip)
    {
        float originalVolume = _audioSource.volume;
        _audioSource.volume = _fxVolume;
        _audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        _audioSource.volume = originalVolume;
    }

    public void StopBG()
    {
        if (_audioSource.isPlaying && _audioSource.loop)
        {
            _audioSource.Stop();
        }
    }

    public void SetBGVolume(float volume)
    {
        _bgVolume = Mathf.Clamp01(volume);
        if (_audioSource.isPlaying && _audioSource.loop)
        {
            _audioSource.volume = _bgVolume;
        }
    }

    public void SetFXVolume(float volume)
    {
        _fxVolume = Mathf.Clamp01(volume);
    }
}
