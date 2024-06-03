using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CookieBattleAward : MonoBehaviour
{
    [SerializeField] private RectTransform _pivotTransform;
    [SerializeField] private SkeletonGraphic[] _skeletonAnimations;

    public void ShowResult()
    {
        List<CookieController> cookies = BattleManager.instance.CookieList;

        SetPosition(cookies);

        for(int i = 0; i < cookies.Count; i++)
        {
            if(cookies[i].CharacterBattleController.IsDead)
            {
                _skeletonAnimations[i].Initialize(true);
                _skeletonAnimations[i].AnimationState.SetAnimation(0, "lose", true);
            }
            else
            {
                _skeletonAnimations[i].Initialize(true);
                _skeletonAnimations[i].AnimationState.SetAnimation(0, "joy", true);
            }
        }
    }

    private void SetPosition(List<CookieController> cookies)
    {
        int cookieCount = 0;

        for(int i = 0; i < _skeletonAnimations.Length; i++)
        {
            _skeletonAnimations[i].gameObject.SetActive(false);
            if(i < cookies.Count)
            {
                cookieCount++;
                _skeletonAnimations[i].gameObject.SetActive(true);
                _skeletonAnimations[i].skeletonDataAsset = ((CookieData)cookies[i].Data).SkeletonDataAsset;
            }
        }
        _pivotTransform.anchoredPosition = new Vector2(-(100 * (2 * cookieCount - 1)) / 2, _pivotTransform.sizeDelta.y);
    }
}
