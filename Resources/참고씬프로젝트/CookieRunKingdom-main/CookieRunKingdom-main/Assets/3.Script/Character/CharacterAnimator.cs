using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Spine.Unity;
using Spine;

public class CharacterAnimator : MonoBehaviour
{
    private BaseController _controller;

    private SkeletonAnimation _animation;
    private Renderer _renderer;
    private BaseSkill _baseSkill;

    private string[] _animationNames;

    public void Init(BaseController baseController)
    {
        _controller = baseController;

        _animation = GetComponentInChildren<SkeletonAnimation>();
        _renderer = _animation.GetComponent<Renderer>();
        _baseSkill = baseController.BaseSkill;

        _animationNames = _controller.Data.AnimationData.Init();

        _animation.Initialize(true);
        _animation.AnimationState.Event += OnSpineEvent;
    }

    /// <summary>
    /// 애니메이션이 재생중인지 확인하는 메소드
    /// </summary>
    /// <returns>재생중이라면 true, 아니면 false</returns>
    public bool IsPlayingAnimation()
    {
        return !_animation.AnimationState.GetCurrent(0).IsComplete;
    }

    public void AdjustmentAnimationName(bool isForward)
    {
        if(!isForward)
        {
            for(int i = 0; i < _animationNames.Length; i++)
            {
                if(_animationNames[i].Contains("_back"))
                {
                    _animationNames[i] = _animationNames[i].Replace("_back", "");
                }

                else if(_animationNames[i].Contains("back"))
                {
                    _animationNames[i] = _animationNames[i].Replace("back", "");
                }
            }
        }
    }

    public void PlayAnimation(ECookieAnimation animationName, bool isLoop = true)
    {
        // 이전 이름과 같으면 return;
        try
        {
            // 이전 이름과 같으면 return;
            if (_animation.AnimationState.GetCurrent(0) != null && _animationNames[(int)animationName] == _animation.AnimationState.GetCurrent(0).Animation.Name)
                return;

            _animation.AnimationState.SetAnimation(0, _animationNames[(int)animationName], isLoop);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.LogError(name + " 에 " + _animationNames[(int)animationName] + " 애니메이션 없습니다.");
        }
    }

    public void PlayAnimation(string animationName, bool isLoop = true)
    {
        try
        {
            // 이전 이름과 같으면 return;
            if (_animation.AnimationState.GetCurrent(0) != null && animationName == _animation.AnimationState.GetCurrent(0).Animation.Name)
                return;

            _animation.AnimationState.SetAnimation(0, animationName, isLoop);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.LogError(name + " 에 " + animationName + " 애니메이션 없습니다.");
        }
    }

    public void FlipX(bool isFlip)
    {
        transform.localScale = new Vector3(isFlip ? -1 : 1, 1, 1);
    }

    public float GetIntervalAnimation()
    {
        return _animation.AnimationState.GetCurrent(0).AnimationEnd -
            _animation.AnimationState.GetCurrent(0).AnimationStart;
    }

    public void SettingOrder()
    {
        _renderer.sortingOrder = -Mathf.RoundToInt(transform.position.y) + 100;
    }

    private void OnSpineEvent(TrackEntry trackEntry, Spine.Event e)
    {
        // 기본공격
        for(int i = 0; i < _baseSkill.attackEvent.Length; i++)
        {
            if(e.Data.Name == _baseSkill.attackEvent[i])
            {
                _baseSkill.NormalAttackEvent();
                return;
            }
        }

        // 스킬 공격
        for(int i = 0; i < _baseSkill.skillEvent.Length; i++)
        {
            if (e.Data.Name == _baseSkill.skillEvent[i])
            {
                _baseSkill.OnSkillEvent(i);
                return;
            }
        }
    }

    public void SettingOrderLayer(bool isHighest)
    {
        if(isHighest)
        {
            _renderer.sortingLayerName = "UIUpper";

        }
        else
        {
            _renderer.sortingLayerName = "Default";
        }
    }
}
