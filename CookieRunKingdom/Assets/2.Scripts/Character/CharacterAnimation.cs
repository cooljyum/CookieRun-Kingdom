using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CharacterAnimation : MonoBehaviour
{
    private SkeletonAnimation skeletonAnimation;
    private CharacterData characterData;

    public void Initialize(CharacterData data, SkeletonAnimation skeleton)
    {
        characterData = data;
        skeletonAnimation = skeleton;
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
}
