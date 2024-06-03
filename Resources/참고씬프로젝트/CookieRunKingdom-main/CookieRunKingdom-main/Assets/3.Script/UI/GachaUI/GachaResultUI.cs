using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GachaResultUI : BaseUI
{
    [SerializeField] private Transform _gachaResultUnitParent;
    [SerializeField] private GachaResultUnit _gachaResultUnitPrefab;

    [SerializeField] private GachaCookieAppear _gachaCookieAppear;

    private List<Vector2> _gachaPercentage = new List<Vector2>();

    private KingdomManager _manager;

    private bool _isGacha = false;
    private int _currentGachaCount;
    private int _maxGachaCount;

    public override void Hide()
    {
        base.Hide();

        _manager.IsMoveCamera = true;
    }

    public override void Init()
    {
        base.Init();

        _manager = FindObjectOfType<KingdomManager>();
    }

    public override void Show()
    {
        base.Show();

        GameManager.Sound.StopBgm();

        _manager.IsMoveCamera = false;

        if (_currentGachaCount != _maxGachaCount)
            Gacha();
        else
            _isGacha = false;
    }

    /// <summary>
    /// 실질적인 가챠의 결과를 처리합니다.
    /// </summary>
    /// <param name="index">해당 쿠키의 인덱스</param>
    /// <param name="isCookie">쿠키인가 소울인가</param>
    /// <param name="count">소울이라면 개수 (1~3)</param>
    private void Gacha(int index, bool isCookie, int count = 0)
    {
        if(!_isGacha)
        {
            _gachaResultUnitParent.DestroyAllChild();
            _isGacha = true;
        }

        GachaResultUnit unit = Instantiate(_gachaResultUnitPrefab, _gachaResultUnitParent);
        unit.Init(index, isCookie, count);

        List<CookieController> cookies = _manager.allCookies;
        CookieData cookieData = ((CookieData)cookies[index].Data);

        CookieStat realCookie = cookies[index].CookieStat;

        if(isCookie)
        {
            // 이미 있는거라면
            if(realCookie.IsHave)
            {
                realCookie.EvolutionGauge += 20;
            }
            else
            {
                realCookie.IsHave = true;
                realCookie.gameObject.SetActive(true);
                cookies[index].CookieCitizeon.StartKingdomAI();
            }
        }
        else
        {
            realCookie.EvolutionGauge += count;
        }

        if(isCookie && ((CookieData)cookies[index].Data).CookieGrade == ECookieGrade.Epic)
        {
            // 쿠키이고 에픽이면 등장 UI 보여줌
            _gachaCookieAppear.InitCookie(cookieData);
            GameManager.UI.PushUI(_gachaCookieAppear);
        }
        else
        {
            if (_currentGachaCount != _maxGachaCount)
                Gacha();
            else
                _isGacha = false;
        }
    }

    public void OnClick(InputAction.CallbackContext value)
    {
        if(value.started)
        {
            if(!_isGacha)
            {
                Debug.Log("나가자");
                GameManager.UI.PopUI();
            }
        }
    }

    public void Gacha10(List<Vector2> gachaPercentage)
    {
        _gachaPercentage = gachaPercentage;
        _currentGachaCount = 0;
        _maxGachaCount = 10;
        Gacha();
    }

    public void Gacha1(List<Vector2> gachaPercentage)
    {
        _gachaPercentage = gachaPercentage;
        _currentGachaCount = 0;
        _maxGachaCount = 1;
        Gacha();
    }

    private void Gacha()
    {
        _currentGachaCount++;

        float choice = Random.Range(0, 100f);

        float sum = 0;
        for (int i = 0; i < _gachaPercentage.Count; i++)
        {
            sum += _gachaPercentage[i].x;
            if (choice <= sum)
            {
                Gacha(i, true);
                return;
            }

            sum += _gachaPercentage[i].y;
            if (choice <= sum)
            {
                int amount = Random.Range(1, 4);
                Gacha(i, false, amount);
                return;
            }
        }
    }
}
