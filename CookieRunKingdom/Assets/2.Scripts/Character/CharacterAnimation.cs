using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Spine.Unity;
using Spine;

public class CharacterAnimation : MonoBehaviour
{
    private SkeletonAnimation _skeletonAni;
    private CharacterData _characterData;

    private Dictionary<string, string> _aniMappingList = new Dictionary<string, string>();

    public event System.Action OnAttackEnd;

    private void Awake()
    {
        // 모든 애니메이션 상태에 이벤트 핸들러 등록
        _skeletonAni.state.Event += HandleAniEvent;
        Debug.Log("Event handler registered in Awake");
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
            _skeletonAni.state.Event += HandleAniEvent;
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
    private void HandleAniEvent(TrackEntry trackEntry, Spine.Event e)
    {
        Debug.Log($"Event: {e.Data.Name}, Animation: {trackEntry.Animation.Name}");
        string status = GetStatusFromAniName(trackEntry.Animation.Name);
     
        if (status == "Battle_Attack")
        {
            OnAttackEnd();
        }
    }
}
