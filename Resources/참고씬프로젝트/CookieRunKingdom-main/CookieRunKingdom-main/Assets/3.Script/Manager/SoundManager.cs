using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public enum EBGM
{
    battle,
    bossBattle,
    defeatBattle,
    gacha,
    lobby,
    mainTitle,
    readyBattle,
    start,
    victoryBattle
}

public class SoundManager
{
    private Dictionary<EBGM, AudioClip> _clipDictionary = new Dictionary<EBGM, AudioClip>();
    private Dictionary<EBGM, string> _clipName = new Dictionary<EBGM, string>();

    private AudioSource _audio = null;
    public float bgmVolume = 0.5f;

    public void LoadStartBGM()
    {
        _clipDictionary = new Dictionary<EBGM, AudioClip>();
        _clipDictionary[EBGM.start] = Resources.Load<AudioClip>("Audio/CKBGM-Start");

        if (_audio == null)
        {
            GameObject go = new GameObject(typeof(SoundManager).Name);
            go.transform.parent = GameManager.Instance.transform;
            _audio = go.AddComponent<AudioSource>();
            _audio.loop = true;
        }

        _audio.volume = bgmVolume;
    }

    public void Init()
    {
        // 오디오 소스 만들어넣기
        // 클립준비해놓기
        _clipName[EBGM.battle] = "Audio/CKBGM-Battle";
        _clipName[EBGM.bossBattle] = "Audio/CKBGM-BossBattle";
        _clipName[EBGM.defeatBattle] = "Audio/CKBGM-DefeatBattle";
        _clipName[EBGM.gacha] = "Audio/CKBGM-Gacha";
        _clipName[EBGM.lobby] = "Audio/CKBGM-Lobby";
        _clipName[EBGM.mainTitle] = "Audio/CKBGM-MainTitle";
        _clipName[EBGM.readyBattle] = "Audio/CKBGM-ReadyBattle";
        _clipName[EBGM.start] = "Audio/CKBGM-Start";
        _clipName[EBGM.victoryBattle] = "Audio/CKBGM-VictoryBattle";
    }

    public void PlayBgm(EBGM bgm)
    {
        if (!_clipDictionary.ContainsKey(bgm))
        {
            LoadClip(bgm, true, PlayClip).Forget();
            return;
        }

        _audio.loop = true;
        StopBgm();
        _audio.clip = _clipDictionary[bgm];
        _audio.Play();
    }

    public void PlaySe(EBGM bgm)
    {
        if (!_clipDictionary.ContainsKey(bgm))
        {
            LoadClip(bgm, false, PlayClip).Forget();
            return;
        }

        _audio.loop = false;
        StopBgm();
        _audio.clip = _clipDictionary[bgm];
        _audio.Play();

    }


    public void StopBgm()
    {
        _audio.Stop();
    }
    
    private void PlayClip(AudioClip clip, bool isLoop = true)
    {
        _audio.loop = isLoop;
        StopBgm();
        _audio.clip = clip;
        _audio.Play();
    }

    private async UniTask LoadClip(EBGM clipType, bool isLoop = true ,System.Action<AudioClip, bool> action = null)
    {
        if(_clipName.ContainsKey(clipType))
        {
            _clipDictionary[clipType] = await Resources.LoadAsync<AudioClip>(_clipName[clipType]) as AudioClip;
            action?.Invoke(_clipDictionary[clipType], isLoop);
        }
        else
        {
            Debug.Log("없는 키 입니다.  " + clipType);
        }
    }
}
