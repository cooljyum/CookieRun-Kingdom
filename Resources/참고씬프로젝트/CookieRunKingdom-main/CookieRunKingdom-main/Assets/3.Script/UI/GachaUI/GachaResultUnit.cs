using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;

public class GachaResultUnit : MonoBehaviour
{
    [SerializeField] private SkeletonGraphic _animation;
    [SerializeField] private Image _unitImage;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private GameObject _highlight;

    public void Init(int index, bool isCookie, int count)
    {
        KingdomManager kingdomManager = FindObjectOfType<KingdomManager>();
        List<CookieController> cookie = kingdomManager.allCookies;

        CookieData cookieData = ((CookieData)cookie[index].Data);

        if(isCookie)
        {
            _animation.enabled = true;
            _unitImage.enabled = false;

            _countText.gameObject.SetActive(false);
            _highlight.SetActive(true);

            _animation.skeletonDataAsset = cookieData.SkeletonDataAsset;
            _animation.Initialize(true);
            _animation.AnimationState.SetAnimation(0, "call_user", true);
        }
        else
        {
            _unitImage.enabled = true;
            _animation.enabled = false;

            _countText.gameObject.SetActive(true);
            _highlight.SetActive(false);

            _unitImage.sprite = cookieData.EvolutionSprite;
            _countText.text = "x" + count;
        }
    }
}
