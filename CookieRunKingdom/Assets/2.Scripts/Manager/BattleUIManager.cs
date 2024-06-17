using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Instance;

    [SerializeField]
    private GameObject _skillBtnPrefab; // 버튼 프리팹
    [SerializeField]
    private Transform panel; // Panel 오브젝트의 Transform

    [SerializeField]
    private float _time;
    [SerializeField]
    private TextMeshProUGUI _timerTxt; // 타임 Text

    [SerializeField]
    private GameObject _stageGauge;

    private bool _isTimerRunning = false;

    private void Awake()
    {
        Instance = this;
        _skillBtnPrefab = Resources.Load<GameObject>("Prefabs/Battle/SkillBtn");
    }

    public GameObject AddSkillBtn()
    {
        GameObject skillBtn = Instantiate(_skillBtnPrefab, panel);
        return skillBtn;
    }

    // This function starts the timer
    public void SetTimer(float time)
    {
        _time = time;
        _isTimerRunning = true;
        SetWidth(0.5f);
    }

    private void Update()
    {
        if (_isTimerRunning)
        {
            UpdateTimer();
        }
    }
    private void UpdateTimer()
    {
        if (_time > 0)
        {
            _time -= Time.deltaTime;
            _timerTxt.text = FormatTime(_time);
        }
        else
        {
            _time = 0;
            _timerTxt.text = "00:00";
            _isTimerRunning = false;
        }
    }

    private string FormatTime(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    private void SetWidth(float normalizedWidth)
    {
        normalizedWidth = Mathf.Clamp01(normalizedWidth);
        float targetWidth = Mathf.Lerp(0f, 380f, normalizedWidth);
        RectTransform rectTransform = _stageGauge.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
        }
        else
        {
            Debug.LogWarning("RectTransform not found on _stageGauge GameObject.");
        }
    }
}
