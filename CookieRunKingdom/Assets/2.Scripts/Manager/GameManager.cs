using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<int> myCards = new List<int>();
    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(this);

        //Load Manager 
        DataManager.Instance.LoadData();
    }

}
