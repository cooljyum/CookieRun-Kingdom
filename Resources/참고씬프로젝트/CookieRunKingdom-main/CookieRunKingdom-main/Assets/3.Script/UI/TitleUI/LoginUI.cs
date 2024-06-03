using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LoginUI : BaseUI
{
    // 로그인 성공할 시 실행될 함수들
    public System.Action onSuccessLogin = null;

    [SerializeField] private TextMeshProUGUI _uiTitle;

    [SerializeField] private TMP_InputField _idField;
    [SerializeField] private TMP_InputField _pwField;

    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _signUpButton;

    [SerializeField] private TextMeshProUGUI _loginButtonText;
    [SerializeField] private TextMeshProUGUI _signUpButtonnText;

    [SerializeField] private TextMeshProUGUI _statusText;

    private Sequence _seq;

    public override void Show()
    {
        base.Show();

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public override void Hide()
    {
        transform.DOScaleY(0, 0.3f).SetEase(Ease.OutBack).OnComplete(base.Hide);
    }

    public override void Init()
    {
        base.Init();

        _uiTitle.text = "로그인";

        _statusText.gameObject.SetActive(false);
        _loginButton.onClick.AddListener(LoginEvent);
        _signUpButton.onClick.AddListener(SignUpEvent);

        _seq = DOTween.Sequence();
        _seq.Pause().SetAutoKill(false).Append(_statusText.transform.DOScale(Vector3.one, 0.2f))
            .SetEase(Ease.OutBack)
            .AppendInterval(0.3f)
            .Append(_statusText.transform.DOScale(Vector3.one, 0.2f))
            .OnComplete(() => _statusText.gameObject.SetActive(false));
    }

    // 로그인 버튼
    public void LoginEvent()
    {
        if(_idField.text.Equals(string.Empty) || _pwField.text.Equals(string.Empty))
        {
            ShowStatus("아이디 비밀번호를 입력해 주세요.");
        }
        else if(!GameManager.SQL.Login(_idField.text, _pwField.text))
        {
            ShowStatus("아이디 비밀번호를 확인해 주세요.");
        }
        else
        {
            GameManager.UI.ExitPopUpUI();
            // 로그인 성공
            onSuccessLogin?.Invoke();
        }
    }

    // 회원가입 버튼
    public void SignUpEvent()
    {
        _idField.text = "";
        _pwField.text = "";

        _loginButton.onClick.RemoveAllListeners();
        _signUpButton.onClick.RemoveAllListeners();

        _loginButton.onClick.AddListener(BackEvent);
        _signUpButton.onClick.AddListener(CreateAccount);

        _uiTitle.text = "회원가입";
        _loginButtonText.text = "돌아가기";
        _signUpButtonnText.text = "계정생성";
    }

    // 계정 생성 버튼
    public void CreateAccount()
    {
        if (_idField.text.Equals(string.Empty) || _pwField.text.Equals(string.Empty))
        {
            ShowStatus("아이디 비밀번호를 입력해 주세요.");
        }
        else if (GameManager.SQL.Login(_idField.text, _pwField.text))
        {
            ShowStatus("이미 존재하는 계정입니다.");
        }
        else
        {
            GameManager.SQL.SignUp(_idField.text, _pwField.text);
            ShowStatus("계정이 생성되었습니다.");
            BackEvent();
        }
    }

    // 돌아가기 버튼
    public void BackEvent()
    {
        _idField.text = "";
        _pwField.text = "";

        _loginButton.onClick.RemoveAllListeners();
        _signUpButton.onClick.RemoveAllListeners();

        _loginButton.onClick.AddListener(LoginEvent);
        _signUpButton.onClick.AddListener(SignUpEvent);

        _uiTitle.text = "로그인";
        _loginButtonText.text = "로그인";
        _signUpButtonnText.text = "회원가입";
    }


    private void ShowStatus(string content)
    {
        _statusText.text = content;
        _statusText.gameObject.SetActive(true);
        _statusText.transform.localScale = Vector3.zero;
        _seq.Pause();
        _seq.Restart();
    }
}
