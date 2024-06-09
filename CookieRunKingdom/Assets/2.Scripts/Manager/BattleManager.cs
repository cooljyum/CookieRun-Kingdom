using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    //BattkeCookie
    [Header("BattleCookie")]
    [Tooltip("Settings related to battle cookies.")]
    int i;

    private void Start()
    {
        BattleObjectSpawnManager.Instance.Init();
    }

   
}
