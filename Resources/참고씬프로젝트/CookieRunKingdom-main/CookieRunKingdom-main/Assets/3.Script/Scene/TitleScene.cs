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
    [SerializeField] private GameObject _communicationUI;

    protected override void Init()
    {
        GameManager.Instance.Init();
        GameManager.Sound.PlaySe(EBGM.start);
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


        // 애니메이션 실행하면 GameManager Load할 때까지 통신UI 켜줌
        _communicationUI.SetActive(true);
        await GameManager.Instance.LoadData();
        _communicationUI.SetActive(false);


        GameManager.Sound.PlayBgm(EBGM.mainTitle);

        // 로그인 UI 보여주자
        GameManager.UI.PushUI(_titleSceneUI);
    }
}
