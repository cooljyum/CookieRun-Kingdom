using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System;

public class BuildingAnimator : MonoBehaviour
{
    private BuildingController _controller;

    private SkeletonAnimation _animation;
    private Renderer _renderer;

    private Renderer[] _renderersInChild;

    public void Init(BuildingController controller)
    {
        _controller = controller;

        _animation = GetComponentInChildren<SkeletonAnimation>();
        _renderer = _animation.GetComponent<Renderer>();
        _renderersInChild = GetComponentsInChildren<Renderer>();
    }

    public bool IsPlayingAnimation()
    {
        return !_animation.AnimationState.GetCurrent(0).IsComplete;
    }

    public void PlayAnimation(string animationName, bool isLoop = true)
    {
        if(_animation.AnimationState != null)
        {
            if (animationName == _animation.AnimationState.GetCurrent(0).Animation.Name)
                return;
        }

        _animation.Initialize(false);
        _animation.AnimationState.SetAnimation(0, animationName, isLoop);
    }

    public void FlipX(bool isFlip)
    {
        _animation.transform.localScale = new Vector3(isFlip ? -1 : 1, 1, 1);
    }

    public float GetIntervalAnimation()
    {
        return _animation.AnimationState.GetCurrent(0).AnimationEnd -
            _animation.AnimationState.GetCurrent(0).AnimationStart;
    }

    public void SettingOrder()
    {
        _renderer.sortingOrder = -Mathf.RoundToInt(transform.position.y) + 101;
    }

    public bool IsAnimationExist(string animationName)
    {
        return _animation.SkeletonDataAsset.GetSkeletonData(true).FindAnimation(animationName) != null;
    }

    public void SettingOrderHigher(bool flag)
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.sortingLayerID = flag ?
            SortingLayer.NameToID("GridUpper") : SortingLayer.NameToID("Default");
        }
    }
}
