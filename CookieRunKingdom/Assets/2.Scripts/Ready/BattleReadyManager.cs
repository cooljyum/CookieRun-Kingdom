using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleReadyManager : MonoBehaviour
{
    public void ExitReady()
    {
        SceneManager.LoadScene(1);
    }
    
    public void EnterBattle()
    {
        SceneManager.LoadScene(3);
    }
}
