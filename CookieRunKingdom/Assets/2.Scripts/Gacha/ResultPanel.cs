using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    private GachaIcon _icon;
    private int _randomValue;
    private void Awake()
    {
        _icon = transform.GetChild(0).GetComponent<GachaIcon>();
        gameObject.SetActive(false);
    }

    public void OpenGacha()
    {
        gameObject.SetActive(true);

        _randomValue = Random.Range(0, 100);
        int _random;

        if (_randomValue < 80)
        {
            _random = Random.Range(0, 5);
            _icon.SetData(_random);
            GameManager.Instance.CurPlayerData.MyCardsLists.Add(_random);
        }        
        else if (_randomValue < 90)
        {
            _icon.SetData(6);
            GameManager.Instance.CurPlayerData.MyCardsLists.Add(6);
        }        
    }

    public void CloseGacha()
    {
        gameObject.SetActive(false);
    }
}
