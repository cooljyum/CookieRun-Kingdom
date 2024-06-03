using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    // init할 때까지 로딩창
    private void Awake()
    {
        Init();
    }

    protected abstract void Init();
}
