using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GachaUI : BaseUI
{
    [Header("배경")]
    [SerializeField] private GameObject _specialGacha;
    [SerializeField] private GameObject _epicGacha;

    [SerializeField] private GachaPercentageUI _gachaPercentageUI;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _diaText;
    [SerializeField] private TextMeshProUGUI _moneyText;

    [SerializeField] private Button _specialGachaButton;
    [SerializeField] private Button _epicGachaButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private Button _gachaButton;
    [SerializeField] private Button _gacha10Button;

    [SerializeField] private Image _gachaImage;
    [SerializeField] private Image _gacha10Image;

    [SerializeField] private TextMeshProUGUI _gachaText;
    [SerializeField] private TextMeshProUGUI _gacha10Text;

    [SerializeField] private Button _detailedProbabilityButton;

    [SerializeField] private GachaResultUI _gachaResultUI;

    [Header("색깔")]
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _idleColor;

    [Header("Sprite")]
    [SerializeField] private Sprite _diaSprite;
    [SerializeField] private Sprite _specialCookieCutterSprite;
    [SerializeField] private Sprite _cookieCutterSprite;

    [Header("Gacha Data")]
    [SerializeField] private GachaData _pickUpGachaData;
    [SerializeField] private GachaData _epicGachaData;

    private GachaCalculator _gachaCalculator;
    private KingdomManager _kingdomManager;

    private List<Vector2> _pickUpGachaPercentage;
    private List<Vector2> _epicGachaPercentage;

    public override void Hide()
    {
        base.Hide();

        _kingdomManager.IsMoveCamera = true;
        GameManager.Sound.PlayBgm(EBGM.lobby);
    }

    public override void Init()
    {
        base.Init();

        _gachaCalculator = GetComponent<GachaCalculator>();
        _kingdomManager = FindObjectOfType<KingdomManager>();

        _specialGachaButton.onClick.AddListener(() => PickUpGacha());
        _epicGachaButton.onClick.AddListener(() => EpicGacha());

        _exitButton.onClick.AddListener(() => GameManager.UI.PopUI());

        GameManager.Game.OnChangeDia += (() => _diaText.text = GameManager.Game.Dia.ToString("#,##0"));
        GameManager.Game.OnChangeMoney += (() => _moneyText.text = GameManager.Game.Money.ToString("#,##0"));

        _pickUpGachaPercentage = _gachaCalculator.CalculateGacha(_pickUpGachaData);
        _epicGachaPercentage = _gachaCalculator.CalculateGacha(_epicGachaData);
    }

    public override void Show()
    {
        base.Show();


        _kingdomManager.IsMoveCamera = false;

        InitSetting();
        PickUpGacha();

        GameManager.Game.UpdateGoods();
        GameManager.Sound.PlayBgm(EBGM.gacha);
    }

    private void InitSetting()
    {
        _specialGacha.SetActive(false);
        _epicGacha.SetActive(false);

        _specialGachaButton.image.color = _idleColor;
        _epicGachaButton.image.color = _idleColor;
    }

    private void PickUpGacha()
    {
        InitSetting();
        _specialGacha.SetActive(true);
        _specialGachaButton.image.color = _selectedColor;

        _gachaImage.sprite = _diaSprite;
        _gachaText.text = GameManager.Game.Dia + "/300";
        _gacha10Image.sprite = _diaSprite;
        _gacha10Text.text = GameManager.Game.Dia + "/3000";

        _detailedProbabilityButton.onClick.RemoveAllListeners();
        _detailedProbabilityButton.onClick.AddListener(() =>
        {
            _gachaPercentageUI.InitData(_pickUpGachaPercentage, _pickUpGachaData);
            GameManager.UI.ShowPopUpUI(_gachaPercentageUI);
        });

        _gachaButton.onClick.RemoveAllListeners();
        _gachaButton.onClick.AddListener(() =>
        {
            // 여기에 다이아 추가 로직
            if(TryGacha(300))
                _gachaResultUI.Gacha1(_pickUpGachaPercentage);
        });

        _gacha10Button.onClick.RemoveAllListeners();
        _gacha10Button.onClick.AddListener(() => 
        {
            if(TryGacha(3000))
                _gachaResultUI.Gacha10(_pickUpGachaPercentage);
        });
    }

    private bool TryGacha(int diaCount)
    {
        if(GameManager.Game.Dia >= diaCount)
        {
            GameManager.Game.Dia -= diaCount;
            GameManager.UI.PushUI(_gachaResultUI);
            return true;
        }
        else
        {
            GuideDisplayer.Instance.ShowGuide("다이아가 부족합니다!");
            return false;
        }
    }


    private void EpicGacha()
    {
        InitSetting();
        _epicGacha.SetActive(true);
        _epicGachaButton.image.color = _selectedColor;

        _gachaImage.sprite = _diaSprite;
        _gachaText.text = GameManager.Game.Dia + "/300";
        _gacha10Image.sprite = _diaSprite;
        _gacha10Text.text = GameManager.Game.Dia + "/3000";

        _detailedProbabilityButton.onClick.RemoveAllListeners();
        _detailedProbabilityButton.onClick.AddListener(() =>
        {
            _gachaPercentageUI.InitData(_epicGachaPercentage, _epicGachaData);
            GameManager.UI.ShowPopUpUI(_gachaPercentageUI);
        });

        _gachaButton.onClick.RemoveAllListeners();
        _gachaButton.onClick.AddListener(() =>
        {
            if(TryGacha(300))
                _gachaResultUI.Gacha1(_epicGachaPercentage);
        });

        _gacha10Button.onClick.RemoveAllListeners();
        _gacha10Button.onClick.AddListener(() =>
        {
            if(TryGacha(3000))
                _gachaResultUI.Gacha10(_epicGachaPercentage);
        });
    }
}
