using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : BaseUI
{
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _cancelButtom;

    public override void Hide()
    {
        base.Hide();
    }

    public override void Init()
    {
        base.Init();

        _exitButton.onClick.AddListener(QuitGame);
        _cancelButtom.onClick.AddListener(() => GameManager.UI.ExitPopUpUI());
    }

    public override void Show()
    {
        base.Show();
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
