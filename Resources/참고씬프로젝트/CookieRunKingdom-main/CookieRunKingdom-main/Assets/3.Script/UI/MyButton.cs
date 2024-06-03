using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyButton : MonoBehaviour
{
    private Action onClick;

    public void AddListener(Action action)
    {
        onClick += action;
    }

    public void RemoveAllListener()
    {
        onClick = null;
    }

    public void OnClick()
    {
        onClick?.Invoke();
    }
}
