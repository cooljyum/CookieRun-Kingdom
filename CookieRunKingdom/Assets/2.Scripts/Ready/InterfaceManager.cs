using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    private TextMeshProUGUI _coinText;
    private TextMeshProUGUI _stageText;

    private void Start()
    {
        _coinText = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        _coinText.text = GameManager.Instance.CurPlayerData.Coin.ToString();
        _stageText = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        _stageText.text = GameManager.Instance.CurPlayerData.CurStage.ToString();
    }    
}
