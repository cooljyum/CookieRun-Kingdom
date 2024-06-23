using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Spine.Unity;
using Spine;
using System.Runtime.Serialization.Formatters;

public class CharacterAnimation : MonoBehaviour
{
    private SkeletonAnimation _skeletonAni;

    private CharacterData _characterData;

    private Dictionary<string, string> _aniMappingList = new Dictionary<string, string>();

    public event System.Action OnAttackEnd;
    public event System.Action OnSkillEnd;

    private void Update()
    {
        TrackEntry trackEntry = _skeletonAni.state.GetCurrent(0);

        if (trackEntry != null)
        {
            // 애니메이션의 총 길이
            float animationDuration = trackEntry.AnimationEnd - trackEntry.AnimationStart;

            if (trackEntry.TrackTime > animationDuration) 
            {
                trackEntry.TrackTime -= animationDuration;
                OnAniEndEvent();
            }
        }
    }

    //캐릭터 애니메이션 Init
    public void Init(CharacterData data, SkeletonAnimation skeletonAni)
    {
        _characterData = data;
        _skeletonAni = skeletonAni;

        foreach (var mapping in _characterData.AnimationMappings)
        {
            _aniMappingList.Add(mapping.Key, mapping.AnimationName);
        }
    }

    //Ani 재생
    public void PlayAni(string curSceneName, string status)
    {
        
        string animationName = GetAniName(curSceneName, status);
        if (!string.IsNullOrEmpty(animationName))
        {
            _skeletonAni.AnimationState.SetAnimation(0, animationName, true);
        }
        else
        {
            Debug.LogWarning($"Not Found! Ani Key: '{status}'");
        }
    }

    //key따른 Ani 재생
    private string GetAniName(string curSceneName, string status)
    {
        string key = curSceneName + "_" + status;

        if (_aniMappingList.TryGetValue(key, out string animationName))
        {
            return animationName;
        }
        return null;
    }

    //AniName에따른 키값 부르기
    private string GetStatusFromAniName(string aniName)
    {
        return _aniMappingList.FirstOrDefault(pair => pair.Value == aniName).Key;
    }

    //Ani
    private void OnAniEndEvent()
    {
        string status = GetStatusFromAniName(_skeletonAni.state.ToString());
        if (status == "Battle_Attack")
        {
            OnAttackEnd();
        }
        else if (status == "Battle_Skill")
        {
            OnSkillEnd();
        }
    }
}


