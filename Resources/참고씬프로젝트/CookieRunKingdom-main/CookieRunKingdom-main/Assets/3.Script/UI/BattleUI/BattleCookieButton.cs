using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleCookieButton : MonoBehaviour
{
    [SerializeField] private Button _battleCookieButton;

    [SerializeField] private Image _portraitImage;
    [SerializeField] private Image _typeImage;

    [SerializeField] private RectTransform _starsParent;
    [SerializeField] private RectTransform[] _stars;

    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private GameObject _emptyText;

    public void UpdateInfo(CookieController cookie)
    {
        if(cookie == null)
        {
            _portraitImage.gameObject.SetActive(false);
            _emptyText.gameObject.SetActive(true);
            return;
        }

        CookieData data = ((CookieData)cookie.Data);

        _portraitImage.gameObject.SetActive(true);
        _emptyText.gameObject.SetActive(false);

        _portraitImage.sprite = data.IdleSprite;
        _typeImage.sprite = data.TypeSprite;

        // º°
        for (int i = 0; i < _stars.Length; i++)
            _stars[i].gameObject.SetActive(false);
        for (int i = 0; i < cookie.CookieStat.EvolutionCount; i++)
            _stars[i].gameObject.SetActive(true);
        _starsParent.anchoredPosition -= (Vector2.right * _stars[0].sizeDelta.x * cookie.CookieStat.EvolutionCount * 0.5f);

        _levelText.text = cookie.CookieStat.CookieLevel.ToString();
    }
}
