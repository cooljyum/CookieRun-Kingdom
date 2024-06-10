using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class CharacterAnimation : MonoBehaviour
{
    private SkeletonAnimation skeletonAnimation;
    private CharacterData characterData;

    public void Init(CharacterData data, SkeletonAnimation skeleton)
    {
        characterData = data;
        skeletonAnimation = skeleton;
        skeletonAnimation.AnimationState.Event += HandleAnimationEvent;
    }

    public void PlayAnimation(string curSceneName, string status)
    {
        string animationName = GetAnimationName(curSceneName, status);
        if (!string.IsNullOrEmpty(animationName))
        {
            skeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
        }
        else
        {
            Debug.LogWarning($"애니메이션 키 '{status}'에 대한 애니메이션 이름을 찾을 수 없습니다.");
        }
    }

    private string GetAnimationName(string curSceneName, string status)
    {
        foreach (var mapping in characterData.AnimationMappings)
        {
            if (mapping.Key == curSceneName + "_" + status)
            {

                return mapping.AnimationName;
            }
        }
        return null;
    }

    public void RegisterAnimationEvent(string animationName, System.Action<TrackEntry, Spine.Event> e)
    {
        skeletonAnimation.AnimationState.Event += (trackEntry, evt) =>
        {
            if (trackEntry.Animation.Name == animationName)
            {
                e(trackEntry, evt);
            }
        };
    }

    private string GetStatusFromAniName(string animationName)
    {
        foreach (var mapping in characterData.AnimationMappings)
        {
            if (mapping.AnimationName == animationName)
            {
                string[] parts = mapping.Key.Split('_');
                if (parts.Length == 2)
                {
                    return parts[1];
                }
            }
        }
        return null;
    }

    private void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    { 
        string status = GetStatusFromAniName(e.Data.Name);
        if (status == "Attack")
        {
            OnAttackComplete?.Invoke();
        }
    }

    public event System.Action OnAttackComplete;
}
