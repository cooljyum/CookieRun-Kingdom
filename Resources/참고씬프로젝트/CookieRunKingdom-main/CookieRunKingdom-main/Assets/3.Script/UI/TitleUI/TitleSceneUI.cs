using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Video;

public class TitleSceneUI : BaseUI
{
    [SerializeField] private LoginUI _loginUI;
    [SerializeField] private CreateNameUI _createNameUI;

    [SerializeField] private TextMeshProUGUI _touchToStartText;
    [SerializeField] private Button _cinematicPlayButton;
    [SerializeField] private Button _screenButton;
    [SerializeField] private Button _skipButton;

    [SerializeField] private VideoPlayer _video;
    private bool _isFirst = false;

    public override void Hide()
    {
        base.Hide();
    }

    public override void Init()
    {
        base.Init();

        _touchToStartText.gameObject.SetActive(false);
        _cinematicPlayButton.gameObject.SetActive(false);
        _screenButton.gameObject.SetActive(false);
        _skipButton.gameObject.SetActive(false);

        _loginUI.onSuccessLogin += OnAfterLoginEvent;
        _createNameUI.onAfterUI += ReadyToStartGame;
        _screenButton.onClick.AddListener(() => StartGame());
        _cinematicPlayButton.onClick.AddListener(() => PlayVideo());

        _skipButton.onClick.AddListener(StopVideo);
        _video.loopPointReached += ((video) => StopVideo());
    }

    public override void Show()
    {
        base.Show();
        GameManager.UI.ShowPopUpUI(_loginUI);
    }

    private void OnAfterLoginEvent()
    {
        GameManager.Game.LoadData();
        DataBaseManager.Instance.LoadData();

        if (GameManager.Game.IsFirst)
        {
            GameManager.Game.IsFirst = false;
            GameManager.UI.ShowPopUpUI(_createNameUI);
            _isFirst = true;
        }
        else
        {
            ReadyToStartGame();
        }
    }

    private void ReadyToStartGame()
    {
        if(_isFirst)
            _cinematicPlayButton.gameObject.SetActive(false);
        else
            _cinematicPlayButton.gameObject.SetActive(true);

        _touchToStartText.gameObject.SetActive(true);
        _screenButton.gameObject.SetActive(true);
        _touchToStartText.DOColor(new Color(0.3f, 0.3f, 0.3f), 1).SetLoops(-1, LoopType.Yoyo);
    }

    private void StartGame()
    {
        if(_isFirst)
        {
            PlayVideo();
        }
        else
        {
            GameManager.Scene.LoadScene(ESceneName.Kingdom);
        }
    }

    private void PlayVideo()
    {
        GameManager.Sound.StopBgm();

        _cinematicPlayButton.gameObject.SetActive(false);
        _touchToStartText.gameObject.SetActive(false);
        _screenButton.gameObject.SetActive(false);
        _touchToStartText.DOKill();

        _skipButton.gameObject.SetActive(true);
        _video.Play();
    }

    private void StopVideo()
    {
        GameManager.Sound.PlayBgm(EBGM.mainTitle);

        _cinematicPlayButton.gameObject.SetActive(true);
        _touchToStartText.gameObject.SetActive(true);
        _screenButton.gameObject.SetActive(true);
        _touchToStartText.DOColor(new Color(0.3f, 0.3f, 0.3f), 1).SetLoops(-1, LoopType.Yoyo);

        _skipButton.gameObject.SetActive(false);
        _video.Stop();


        if (_isFirst)
            GameManager.Scene.LoadScene(ESceneName.Kingdom);
    }
}
