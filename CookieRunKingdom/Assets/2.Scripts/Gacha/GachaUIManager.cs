using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GachaUIManager : MonoBehaviour
{
    private TextMeshProUGUI _coinText;  
    private TextMeshProUGUI _mileageText;

    private void Start()
    {
        _coinText = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        _coinText.text = GameManager.Instance.CurPlayerData.Coin.ToString();
        _mileageText = transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        _mileageText.text = GameManager.Instance.CurPlayerData.Mileage.ToString();
    }
    private void Update()
    {
        _coinText.text = GameManager.Instance.CurPlayerData.Coin.ToString();
        _mileageText.text = GameManager.Instance.CurPlayerData.Mileage.ToString();
    }
}
