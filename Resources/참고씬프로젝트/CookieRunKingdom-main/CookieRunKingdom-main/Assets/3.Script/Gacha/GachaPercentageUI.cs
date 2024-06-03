using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GachaPercentageUI : BaseUI
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _epicPercentageText;
    [SerializeField] private TextMeshProUGUI _rarePercentageText;
    [SerializeField] private TextMeshProUGUI _commonPercentageText;
    [SerializeField] private Button _exitButton;

    [Header("Color")]
    [SerializeField] private Color _epicColor;
    [SerializeField] private Color _rareColor;
    [SerializeField] private Color _commonColor;
    [SerializeField] private Color _epicBaseColor;
    [SerializeField] private Color _rareBaseColor;
    [SerializeField] private Color _commonBaseColor;

    [Header("Prefab")]
    [SerializeField] private RectTransform _unitParent;
    [SerializeField] private GachaUnitUI _gachaUnitUIPrefab;

    private KingdomManager _manager;

    private GachaData _gachaData;
    private List<Vector2> _gachaPercentage;

    private List<GachaUnitUI> cookieGachaUnitList = new List<GachaUnitUI>();
    private List<GachaUnitUI> soulGachaUnitList = new List<GachaUnitUI>();

    public void InitData(List<Vector2> gachaPercentage, GachaData gachaData)
    {
        _gachaPercentage = gachaPercentage;
        _gachaData = gachaData;
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Init()
    {
        base.Init();
        _manager = FindObjectOfType<KingdomManager>();
        _exitButton.onClick.AddListener(() => GameManager.UI.ExitPopUpUI());
    }

    public override void Show()
    {
        base.Show();

        _unitParent.DestroyAllChild();
        List<CookieController> allCookie = _manager.allCookies;

        cookieGachaUnitList = new List<GachaUnitUI>();
        soulGachaUnitList = new List<GachaUnitUI>();

        for(int i = 0; i < allCookie.Count; i++)
        {
            // ÄíÅ°
            GachaUnitUI gachaCookieUnit = Instantiate(_gachaUnitUIPrefab, _unitParent);
            gachaCookieUnit.Init(((CookieData)allCookie[i].Data), false, _gachaPercentage[i].x, GetGradeColor(allCookie[i]), GetBaseColor(allCookie[i]));
            cookieGachaUnitList.Add(gachaCookieUnit);

            // ¿µÈ¥¼®
            GachaUnitUI gachaSoulUnit = Instantiate(_gachaUnitUIPrefab, _unitParent);
            gachaSoulUnit.Init(((CookieData)allCookie[i].Data), true, _gachaPercentage[i].y, GetGradeColor(allCookie[i]), GetBaseColor(allCookie[i]));
            soulGachaUnitList.Add(gachaSoulUnit);
        }

        // È®·ü ¿À¸§Â÷¼øÀ¸·Î Á¤·Ä
        cookieGachaUnitList.Sort((a, b) => (a.Percentage).CompareTo(b.Percentage));
        soulGachaUnitList.Sort((a, b) => (a.Percentage).CompareTo(b.Percentage));

        for(int i = 0; i < allCookie.Count; i++)
        {
            cookieGachaUnitList[i].transform.SetSiblingIndex(i * 2);
            soulGachaUnitList[i].transform.SetSiblingIndex(i * 2 + 1);
        }

        // ÇÈ¾÷ÄíÅ°°¡ ÀÖ´Ù¸é Á¦ÀÏ À§·Î ¿Ã¸®±â
        if(_gachaData.IsPickUp)
        {
            for(int i = 0; i < cookieGachaUnitList.Count; i++)
            {
                if(cookieGachaUnitList[i].CookieName == allCookie[_gachaData.PickUpCookie].Data.CharacterName)
                {
                    cookieGachaUnitList[i].transform.SetSiblingIndex(0);
                    cookieGachaUnitList[i].ShowPickUp();
                    break;
                }
            }

            for(int i = 0; i < soulGachaUnitList.Count; i++)
            {
                if (soulGachaUnitList[i].CookieName == allCookie[_gachaData.PickUpCookie].Data.CharacterName)
                {
                    soulGachaUnitList[i].transform.SetSiblingIndex(1);
                    break;
                }
            }

            for(int i = 0; i < cookieGachaUnitList.Count; i++)
            {
                if(cookieGachaUnitList[i].CookieName == allCookie[_gachaData.PickUpCookie].Data.CharacterName)
                {
                    cookieGachaUnitList[i].transform.SetSiblingIndex(0);
                    break;
                }
            }
        }

        _titleText.text = _gachaData.IsPickUp ? "½ºÆä¼È ÄíÅ° »Ì±â ¼¼ºÎ È®·ü" : "°í±Þ ÄíÅ° »Ì±â ¼¼ºÎ È®·ü";
        _epicPercentageText.text = _gachaData.EpicPercentage.ToString("F3") + "%";
        _rarePercentageText.text = _gachaData.RarePercentage.ToString("F3") + "%";
        _commonPercentageText.text = _gachaData.CommonPercentage.ToString("F3") + "%";


        _unitParent.sizeDelta = new Vector2(_unitParent.sizeDelta.x, 10 + allCookie.Count * 2 * 110);
    }


    private Color GetGradeColor(CookieController cookie)
    {
        ECookieGrade grade = ((CookieData)cookie.Data).CookieGrade;
        switch (grade)
        {
            case ECookieGrade.Common:
                return _commonColor;
            case ECookieGrade.Rare:
                return _rareColor;
            case ECookieGrade.Epic:
                return _epicColor;
        }
        return Color.white;
    }

    private Color GetBaseColor(CookieController cookie)
    {
        ECookieGrade grade = ((CookieData)cookie.Data).CookieGrade;
        switch (grade)
        {
            case ECookieGrade.Common:
                return _commonBaseColor;
            case ECookieGrade.Rare:
                return _rareBaseColor;
            case ECookieGrade.Epic:
                return _epicBaseColor;
        }
        return Color.white;
    }
}
