using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ESceneName
{
    Title,
    Kingdom,
    Battle
}

public class SceneManagerEx
{
    // 씬 이동시 실행할 함수들
    public delegate void OnMoveOtherScene();
    public OnMoveOtherScene onMoveOtherScene;

    private LoadingUI _loadingUI;

    public ESceneName CurrentScene { get; private set; }

    public void Init()
    {
        _loadingUI = GameObject.FindObjectOfType<LoadingUI>();
        _loadingUI.transform.SetParent(GameManager.Instance.transform);
        CurrentScene = ESceneName.Title;
        GameManager.UI.InsertUI(_loadingUI.transform);
    }

    public void EndLoading()
    {
        _loadingUI.EndLoading();
    }

    public void LoadScene(ESceneName sceneName)
    {
        GameManager.Sound.StopBgm();

        _loadingUI.StartLoading();

        onMoveOtherScene?.Invoke();
        SceneManager.LoadScene((int)sceneName);

        CurrentScene = sceneName;

        // 씬 이동 시 UI들 제거
        GameManager.UI.ClearUI();
    }
}
