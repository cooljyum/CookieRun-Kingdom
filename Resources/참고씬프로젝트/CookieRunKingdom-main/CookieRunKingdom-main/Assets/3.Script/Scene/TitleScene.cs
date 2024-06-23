using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.InputSystem;

using Cysharp.Threading.Tasks;


public class TitleScene : BaseScene
{
    [SerializeField] private SkeletonAnimation _titleAnimation;
    [SerializeField] private SkeletonAnimation _titleLogoAnimation;
    [SerializeField] private TitleSceneUI _titleSceneUI;

    protected override void Init()
    {
      //  GameManager.Sound.PlaySe(EBGM.start);
        TitleLogic().Forget();
    }


    private async UniTaskVoid TitleLogic()
    {
        // 애니메이션 실행
        _titleAnimation.AnimationState.SetAnimation(0, "start", false);

        await UniTask.WaitUntil(() => _titleAnimation.AnimationState.GetCurrent(0).IsComplete);

        _titleLogoAnimation.gameObject.SetActive(true);
        _titleAnimation.AnimationState.SetAnimation(0, "idle", true);
        _titleLogoAnimation.AnimationState.SetAnimation(0, "ko", false);

        await UniTask.WaitUntil(() => _titleLogoAnimation.AnimationState.GetCurrent(0).IsComplete);

    }
}
