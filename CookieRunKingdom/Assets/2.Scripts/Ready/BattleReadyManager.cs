using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleReadyManager : MonoBehaviour
{
    private void OnEnable()
    {
       
    }
    public void ExitReady()
    {
        SoundManager.Instance.PlayFX("BtnClick");
        SceneManager.LoadScene(1);
    }
    
    public void EnterBattle()
    {
        SoundManager.Instance.PlayFX("BtnClick");
        SceneManager.LoadScene(3);
    }
}
