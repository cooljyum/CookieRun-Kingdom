using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SelectCraftCookieButton : MonoBehaviour
{
    [SerializeField] private Button _selectCraftCookieButton;

    [SerializeField] private Image _portraitImage;
    [SerializeField] private Image _typeImage;
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private RectTransform _starParent;
    [SerializeField] private RectTransform[] _stars;

    private CookieController _cookie;
    private CookieData _data;
    private Vector3 _originStarsPosition;

    public void InitInfo(CookieController cookie, System.Action<CookieController> action = null)
    {
        _cookie = cookie;
        _data = ((CookieData)_cookie.Data);

        UpdataInfo();

        // 버튼 온클릭 재할당
        _selectCraftCookieButton.onClick.RemoveAllListeners();
        _selectCraftCookieButton.onClick.AddListener(() => action(_cookie));

        _originStarsPosition = Vector2.zero;
    }

    public void UpdataInfo()
    {
        _portraitImage.sprite = _data.IdleSprite;
        _typeImage.sprite = _data.TypeSprite;
        _levelText.text = _cookie.CookieStat.CookieLevel.ToString();


        // 별찍기 
        for (int i = 0; i < _stars.Length; i++)
            _stars[i].gameObject.SetActive(false);

        for (int i = 0; i < _cookie.CookieStat.EvolutionCount; i++)
            _stars[i].gameObject.SetActive(true);
        _starParent.anchoredPosition = _originStarsPosition - (Vector3.right * _stars[0].sizeDelta.x * _cookie.CookieStat.EvolutionCount * 0.5f);
    }
}
