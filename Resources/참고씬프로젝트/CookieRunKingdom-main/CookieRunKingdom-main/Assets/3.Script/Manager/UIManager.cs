using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private Canvas _rootCanvas;

    public void Init()
    {
        _rootCanvas = GameObject.Instantiate(Resources.Load<Canvas>("LoadingScene/RootCanvas"), GameManager.Instance.transform);
    }

    private Stack<BaseUI> uiStack = new Stack<BaseUI>();

    public void ShowPopUpUI(BaseUI ui)
    {
        uiStack.Push(ui);
        uiStack.Peek().gameObject.SetActive(true);
        uiStack.Peek().Show();
    }

    public void InsertUI(Transform ui)
    {
        ui.SetParent(_rootCanvas.transform);
    }

    public void ExitPopUpUI()
    {
        if (uiStack.Count != 0)
        {
            uiStack.Peek().Hide();
            uiStack.Pop();
        }
    }

    public void PushUI(BaseUI ui)
    {
        if (uiStack.Count != 0)
            uiStack.Peek().Hide();

        uiStack.Push(ui);
        uiStack.Peek().gameObject.SetActive(true);
        uiStack.Peek().Show();
    }

    public void PopUI()
    {
        if(uiStack.Count == 0)
        {
            Debug.LogError("더이상 UI가 없습니다.");
            return;
        }

        uiStack.Peek().Hide();
        uiStack.Pop();

        if(uiStack.Count != 0)
        {
            uiStack.Peek().gameObject.SetActive(true);
            uiStack.Peek().Show();
        }
    }

    public void ClearUI()
    {
        while(uiStack.Count != 0)
        {
            uiStack.Peek().Hide();
            uiStack.Pop();
        }
    }
}
