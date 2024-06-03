using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyCookieButtonUI : MonoBehaviour
{
    [SerializeField] private Button myCookieButton;

    [SerializeField] private Image _portraitImage;
    [SerializeField] private Image _typeImage;
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private RectTransform _startParent;
    [SerializeField] private RectTransform[] _stars;

    [SerializeField] private Slider _evolutionGauge;
    [SerializeField] private TextMeshProUGUI _evolutionGaugeText;
    [SerializeField] private Image _evolutionImage;

    [SerializeField] private GameObject _levelUI;
    [SerializeField] private GameObject _lockedUI;

    private CookieController _cookie;
    private CookieData _data;

    private Vector3 _originStarsPosition;

    public void InitInfo(CookieController cookie, System.Action<CookieController> action = null)
    {
        _cookie = cookie;
        _data = ((CookieData)_cookie.Data);

        UpdateInfo();

        myCookieButton.onClick.AddListener(() => action(_cookie));

        _originStarsPosition = Vector2.zero;
    }

    public void UpdateInfo()
    {
        for (int i = 0; i < _stars.Length; i++)
            _stars[i].gameObject.SetActive(false);

        // 보유하고 있다면
        if (_cookie.CookieStat.IsHave)
        {
            _portraitImage.sprite = _data.IdleSprite;
            _typeImage.sprite = _data.TypeSprite;
            _levelText.text = _cookie.CookieStat.CookieLevel.ToString();

            _evolutionGauge.value = (float)_cookie.CookieStat.EvolutionGauge / _cookie.CookieStat.EvolutionMaxGauge;
            _evolutionGaugeText.text = _cookie.CookieStat.EvolutionGauge + "/" + _cookie.CookieStat.EvolutionMaxGauge;
            _evolutionImage.sprite = _data.EvolutionSprite;

            _levelUI.SetActive(true);
            _lockedUI.SetActive(false);

            // 별
            for(int i = 0; i < _cookie.CookieStat.EvolutionCount; i++)
                _stars[i].gameObject.SetActive(true);
            _startParent.anchoredPosition = _originStarsPosition - (Vector3.right * _stars[0].sizeDelta.x * _cookie.CookieStat.EvolutionCount * 0.5f);
        }
        // 보유하고 있지 않다면
        else
        {
            _portraitImage.sprite = _data.IdleBlackAndWhiteSprite;
            _typeImage.sprite = _data.TypeSprite;
            _levelText.text = "";

            _evolutionGauge.value = (float)_cookie.CookieStat.EvolutionGauge / _cookie.CookieStat.EvolutionMaxGauge;
            _evolutionGaugeText.text = _cookie.CookieStat.EvolutionGauge + "/" + _cookie.CookieStat.EvolutionMaxGauge;
            _evolutionImage.sprite = _data.EvolutionSprite;

            _levelUI.SetActive(false);
            _lockedUI.SetActive(true);

            // 별
            for (int i = 0; i < _cookie.CookieStat.EvolutionCount; i++)
                _stars[i].gameObject.SetActive(true);

            _startParent.anchoredPosition = _originStarsPosition - (Vector3.right * _stars[0].sizeDelta.x * _cookie.CookieStat.EvolutionCount * 0.5f);
        }
    }
}
