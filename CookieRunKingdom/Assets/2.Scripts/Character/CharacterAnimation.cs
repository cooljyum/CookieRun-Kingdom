using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Spine.Unity;
using Spine;

public class CharacterAnimation : MonoBehaviour
{
    private SkeletonAnimation _skeletonAnimation;
    private CharacterData _characterData;

    private Dictionary<string, string> _aniMappingList = new Dictionary<string, string>();

    public event System.Action OnAttackComplete;

    public void Init(CharacterData data, SkeletonAnimation skeleton)
    {
        _characterData = data;
        _skeletonAnimation = skeleton;
        _skeletonAnimation.state.Event += HandleAnimationEvent;

        foreach (var mapping in _characterData.AnimationMappings)
        {
            _aniMappingList.Add(mapping.Key, mapping.AnimationName);
        }
    }

    public void PlayAnimation(string curSceneName, string status)
    {
        string animationName = GetAnimationName(curSceneName, status);
        if (!string.IsNullOrEmpty(animationName))
        {
            _skeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
        }
        else
        {
            Debug.LogWarning($"애니메이션 키 '{status}'에 대한 애니메이션 이름을 찾을 수 없습니다.");
        }
    }

    private string GetAnimationName(string curSceneName, string status)
    {
        string key = curSceneName + "_" + status;
        if (_aniMappingList.TryGetValue(key, out string animationName))
        {
            return animationName;
        }
        return null;
    }

    private string GetStatusFromAniName(string aniName)
    {
        return _aniMappingList.FirstOrDefault(pair => pair.Value == aniName).Key;
    }

    private void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        string status = GetStatusFromAniName(trackEntry.Animation.Name);
        if (status == "Battle_Attack")
        {
            OnAttackComplete?.Invoke();
        }
    }
}
