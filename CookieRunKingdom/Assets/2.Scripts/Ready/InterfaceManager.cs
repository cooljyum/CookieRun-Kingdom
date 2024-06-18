using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    private TextMeshProUGUI _coinText;

    private void Awake()
    {
        _coinText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        _coinText.text = GameManager.Instance.CurPlayerData.Level.ToString();
    }
}
