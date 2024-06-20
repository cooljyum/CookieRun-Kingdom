using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Instance;

    [Header("SKillBtn")]
    [SerializeField]
    private GameObject _skillBtnPrefab;
    [SerializeField]
    private Transform _skillBtnParentTransform;

    [Header("Timer")]
    [SerializeField]
    private bool _isTimerRunning = false;
    [SerializeField]
    private float _battleTime;
    public float BattleTime 
    {
        get { return _battleTime; } 
    }
    [SerializeField]
    private TextMeshProUGUI _timerTxt;

    [Header("StageGuage")]
    [SerializeField]
    private RectTransform _stageGauge;
    private float _prevGauge = 0;

    [Header("StageSpeed")]
    [SerializeField]
    private Button _stageSpeedBtn;
    private TextMeshProUGUI _stageSpeedBtnTxt;
    private int _speedState = 0;
    private float _stageSpeed = 1;
    public float StageSpeed
    {
        get { return _stageSpeed; }
    }

    [Header("StopBtn")]
    [SerializeField]
    private Button _stopBtn;
    [SerializeField]
    private GameObject _stopUI;
    [SerializeField]
    private Button _exitBtn;
    [SerializeField]
    private Button _continueBtn;

    [Header("ResultUIController")]
    [SerializeField]
    private ResultUIController _resultUIController;

    private void Awake()
    {
        Instance = this;
        _skillBtnPrefab = Resources.Load<GameObject>("Prefabs/Battle/SkillBtn");
        _stageSpeedBtnTxt = _stageSpeedBtn.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void Init()
    {
        SetGaugeWidth(0f);

        //Setting Btn Click Event
        if (_stageSpeedBtn != null)
        {
            _stageSpeedBtn.onClick.AddListener(OnStageSpeedBtnClick);
        }
        if (_stopBtn != null)
        {
            _stopBtn.onClick.AddListener(() => ToggleObj(_stopUI, true));
            _stopBtn.onClick.AddListener(() => BattleManager.Instance.IsStop = true);
        }
        if (_exitBtn != null)
        {
            _exitBtn.onClick.AddListener(() => ToggleObj(_stopUI, false));
            _exitBtn.onClick.AddListener(() => BattleManager.Instance.IsStop = false);
            _exitBtn.onClick.AddListener(() => SceneManager.LoadScene("ReadyScene"));
        }
        if (_continueBtn != null)
        {
            _continueBtn.onClick.AddListener(() => ToggleObj(_stopUI, false));
            _continueBtn.onClick.AddListener(() => BattleManager.Instance.IsStop = false);
        }

        //Setting Obj Active
        _stopUI.SetActive(false);

        _resultUIController.Init();
    }

    public GameObject AddSkillBtn()
    {
        GameObject skillBtn = Instantiate(_skillBtnPrefab, _skillBtnParentTransform);
        return skillBtn;
    }

    // 타이머 설정 
    public void SetTimer(float time)
    {
        _battleTime = time;
        _isTimerRunning = true;
    }

    private void Update()
    {
        if (_isTimerRunning)
        {
            if (BattleManager.Instance.IsStop) return;

            UpdateTimer();

            if (!BattleManager.Instance.IsOnBattle) 
            {
                //UI게이지 추가
                float targetGauge = BattleManager.Instance.KillEnemiesGuage;
                if (targetGauge > _prevGauge)
                {
                    _prevGauge += 0.1f * Time.deltaTime; // 게이지가 점진적으로 증가
                    SetGaugeWidth(_prevGauge);
                }
            }
        }
    }
    private void UpdateTimer()
    {
        if (_battleTime > 0)
        {
            _battleTime -= Time.deltaTime * StageSpeed;
            _timerTxt.text = FormatTime(_battleTime);
        }
        else
        {
            _battleTime = 0;
            _timerTxt.text = "00:00";
            _isTimerRunning = false;

            SetResultUI(false);
        }
    }

    private string FormatTime(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    public void SetGaugeWidth(float normalizedWidth)
    {
        normalizedWidth = Mathf.Clamp01(normalizedWidth);
        float targetWidth = Mathf.Lerp(0f, 380f, normalizedWidth);

        if (_stageGauge != null)
        {
            _stageGauge.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
        }
    }

    public float GetGaugeWidth()
    {
        if (_stageGauge != null)
        {
            return _stageGauge.rect.width;
        }
        return 0f;
    }

    private void OnStageSpeedBtnClick()
    {
        _speedState = (_speedState + 1) % 3;

        switch (_speedState)
        {
            case 0:
                _stageSpeed = 1.0f;
                _stageSpeedBtn.transform.GetChild(1).gameObject.SetActive(false);
                break;
            case 1:
                _stageSpeed = 1.2f;
                _stageSpeedBtn.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case 2:
                _stageSpeed = 1.5f;
                _stageSpeedBtn.transform.GetChild(1).gameObject.SetActive(true);
                break;
        }

        _stageSpeedBtnTxt.text = "x" + _stageSpeed.ToString("F1");
    }

    private void ToggleObj(GameObject panel, bool state)
    {
        if (panel != null)
        {
            panel.SetActive(state);
        }
    }

    public void SetResultUI(bool isWin)
    {
        BattleManager.Instance.IsStop = true;
        _resultUIController.SetResultUI(isWin);
    }
}
