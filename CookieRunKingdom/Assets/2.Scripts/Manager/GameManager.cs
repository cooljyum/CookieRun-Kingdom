using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        DataManager.Instance.LoadData();
    }
}
