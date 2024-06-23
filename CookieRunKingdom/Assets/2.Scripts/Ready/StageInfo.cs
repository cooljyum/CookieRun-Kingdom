using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfo : MonoBehaviour
{
    private Image _coinImage;
    private Image _expImage;
    private TextMeshProUGUI _coinText;
    private TextMeshProUGUI _expText;
        
    // Start is called before the first frame update
    void Start()
    {
        _coinImage = transform.GetChild(2).GetComponent<Image>();
        _coinText = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();        
        _expImage = transform.GetChild(3).GetComponent<Image>();
        _expText = transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
            
        int stage = GameManager.Instance.CurPlayerData.CurStage;
        List<StageItem> itemDatas = DataManager.Instance.GetStageData(stage).StageItemList;

        _coinText.text = itemDatas[1].Value.ToString();
        _coinImage.sprite = DataManager.Instance.GetItemData(itemDatas[1].Key).Sprite;
                
        _expText.text = itemDatas[0].Value.ToString();
        _expImage.sprite = DataManager.Instance.GetItemData(itemDatas[0].Key).Sprite;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
