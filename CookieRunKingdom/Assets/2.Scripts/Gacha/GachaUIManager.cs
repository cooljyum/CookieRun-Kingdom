using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GachaUIManager : MonoBehaviour
{
    private TextMeshProUGUI _coinText;    

    private void Start()
    {
        _coinText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        _coinText.text = GameManager.Instance.CurPlayerData.Coin.ToString();        
    }
}
