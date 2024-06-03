using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CreateNameUI : BaseUI
{
    public System.Action onAfterUI = null;

    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TMP_InputField _kingdomNameField;
    [SerializeField] private Button _startButton;

    private Sequence seq;

    public override void Hide()
    {
        transform.DOScaleY(0, 0.3f).SetEase(Ease.OutBack).OnComplete(base.Hide);
        onAfterUI?.Invoke();
    }

    public override void Init()
    {
        base.Init();

        seq = DOTween.Sequence();
        seq.Pause().SetAutoKill(false).OnStart(() => _titleText.color = Color.black)
        .Append(_titleText.transform.DOScale(Vector3.one * 1.1f, 0.1f))
        .Append(_titleText.transform.DOScale(Vector3.one, 0.1f))
        .OnComplete(() => _titleText.color = Color.red);

        _startButton.onClick.AddListener(StartButton);
    }

    public override void Show()
    {
        base.Show();
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void StartButton()
    {
        if (_kingdomNameField.text.Equals(string.Empty))
        {
            seq.Pause();
            seq.Restart();
        }
        else
        {
            // 이름 저장해줘

            GameManager.Game.KingdomName = _kingdomNameField.text;

            GameManager.UI.ExitPopUpUI();
        }
    }
}

