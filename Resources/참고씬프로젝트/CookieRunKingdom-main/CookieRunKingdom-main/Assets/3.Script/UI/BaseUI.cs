using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    private bool _isInit = false;

    public virtual void Show()
    {
        if (!_isInit)
            Init();
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Init()
    {
        _isInit = true;
    }
}
