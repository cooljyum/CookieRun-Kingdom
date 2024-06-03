using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CookieReadyAdventureUI : BaseUI
{
    [Header("UI")]
    [SerializeField] private CookieReadyEditUI _cookieReadyEditUI;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _startBattleButton;
    [SerializeField] private Button _settingTeamButton;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _diaText;
    [SerializeField] private TextMeshProUGUI _jellyText;
    [SerializeField] private JellyInfoUI _jellyInfo;
    [SerializeField] private TextMeshProUGUI _jellyCountText; // 필요한 젤리 개수
    [SerializeField] private TextMeshProUGUI _powerValueText;
    [SerializeField] private TextMeshProUGUI _enemyPowerValueText;

    [Header("EnemySlot")]
    [SerializeField] private EnemySlot[] _enemySlots; 

    [SerializeField] private Transform _cookiePosition;

    private KingdomManager _manager;
    private CookieSelectUI _cookieSelectUI;
    private Vector3 _cookiePositionOrigin;

    public override void Hide()
    {
        base.Hide();
    }

    public override void Init()
    {
        base.Init();

        GameManager.Game.OnChangeDia += (() => _diaText.text = GameManager.Game.Dia.ToString("#,##0"));
        GameManager.Game.OnChangeJelly += (() => _jellyText.text = GameManager.Game.Jelly + "/" + GameManager.Game.MaxJelly);
        GameManager.Game.OnChangeJelly += (() => _jellyInfo.OnChangeJelly());

        _manager = FindObjectOfType<KingdomManager>();
        _cookieSelectUI = FindObjectOfType<CookieSelectUI>();

        _cookiePositionOrigin = _cookiePosition.position;
        BindingButton();
    }

    public override void Show()
    {
        base.Show();

        StageData data = GameManager.Game.StageData;

        _cookiePosition.position = _cookiePositionOrigin;
        _jellyCountText.text = (-data.Jelly).ToString();

        int power = 0;
        List<CookieController> cookies = _manager.allCookies;
        for (int i = 0; i < cookies.Count; i++)
            if (cookies[i].CookieStat.IsBattleMember)
                power += cookies[i].CharacterStat.powerStat;
        _powerValueText.text = power.ToString();

        _enemyPowerValueText.text = data.RecommendedPower.ToString();

        GameManager.Game.UpdateGoods();


        // 몬스터 보여주자
        HashSet<EnemyData> enemiesData = new HashSet<EnemyData>();

        for(int i = 0; i < data.WaveInfo.Length; i++)
            for(int j = 0; j < data.WaveInfo[i].enemies.Length; j++)
                if (data.WaveInfo[i].enemies[j] != null)
                {
                    CharacterData characterData = data.WaveInfo[i].enemies[j].Data;
                    enemiesData.Add((EnemyData)characterData);
                }
        List<EnemyData> enemyList = new List<EnemyData>();

        foreach(EnemyData enemyData in enemiesData)
            enemyList.Add(enemyData);

        enemyList.Sort((data1, data2) => (data2.Power).CompareTo(data1.Power));

        for (int i = 0; i < _enemySlots.Length; i++)
        {
            if(i < enemyList.Count)
            {
                _enemySlots[i].gameObject.SetActive(true);
                _enemySlots[i].UpdateUI(enemyList[i]);
            }
            else
            {
                _enemySlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void BindingButton()
    {
        _settingTeamButton.onClick.AddListener(() =>
        {
            GameManager.UI.ExitPopUpUI();
            GameManager.UI.ShowPopUpUI(_cookieReadyEditUI);
        });

        _exitButton.onClick.AddListener(() =>
        {
            GameManager.UI.ExitPopUpUI();
            GameManager.UI.PopUI();
        });

        _startBattleButton.onClick.AddListener(() =>
        {
            _cookieSelectUI.OnClickBattleStartButton();
        });
    }
}
