using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftItemUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _materialSetting;
    [SerializeField]
    private GameObject _equipmentSetting;
    [Header("Information")]
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private TextMeshProUGUI _itemNameText;
    [SerializeField]
    private TextMeshProUGUI _requiredTimeText;
    [Header("Material")]
    [SerializeField]
    private TextMeshProUGUI _costText;
    [SerializeField]
    private Image _resultImage;
    [SerializeField]
    private TextMeshProUGUI _resultAmountText;
    [Header("Equipment")]
    [SerializeField]
    private Image _requiredMaterial1Image;
    [SerializeField]
    private TextMeshProUGUI _requiredMaterial1AmountText;
    [SerializeField]
    private GameObject _material2Object;
    [SerializeField]
    private Image _requiredMaterial2Image;
    [SerializeField]
    private TextMeshProUGUI _requiredMaterial2AmountText;
    [SerializeField]
    private TextMeshProUGUI _curEquipmentAmount;

    private CraftItemInfo _craftItemInfo;
    
    public void SetData(CraftItemInfo craftItemInfo) //생성한 Craft 칸 데이터 세팅
    {
        _craftItemInfo = craftItemInfo;

        _itemImage.sprite = craftItemInfo.ResultItem.Sprite;
        _itemNameText.text = craftItemInfo.ResultItem.Name;
        _requiredTimeText.text = TimeManager.ConvertTime(craftItemInfo.RequiredTime);

        if (craftItemInfo.IsMaterial) //재료 세팅
        {
            _materialSetting.SetActive(true);
            _equipmentSetting.SetActive(false);
            _costText.text = craftItemInfo.Cost.ToString();
            _resultImage.sprite = GetItemData(craftItemInfo.ResultItem.MaterialKeys[0]).Sprite;
            _resultAmountText.text = "X" + craftItemInfo.ResultCount.ToString();            
        }
        else //도구 세팅
        {
            _materialSetting.SetActive(false);
            _equipmentSetting.SetActive(true);

            int material1Key = craftItemInfo.ResultItem.MaterialKeys[0];
            ItemData material1Data = GetItemData(material1Key);
            int material1Count = GameManager.Instance.PlayerInventory.GetItemCount(material1Key);

            //재료1 데이터 존재 확인
            if (material1Data != null)
            {
                _requiredMaterial1Image.sprite = material1Data.Sprite;
                _requiredMaterial1AmountText.text = material1Count.ToString() + "/" + craftItemInfo.ResultItem.MaterialAmounts[0].ToString();
            }
            else
            {
                Debug.LogWarning("Material 1 data not found.");
            }

            //재료 데이터 개수 확인
            if (craftItemInfo.ResultItem.MaterialKeys.Count > 1)
            {
                int material2Key = craftItemInfo.ResultItem.MaterialKeys[1];
                ItemData material2Data = GetItemData(material2Key);
                int material2Count = GameManager.Instance.PlayerInventory.GetItemCount(material2Key);

                //재료2 데이터 존재 확인
                if (material2Data != null)
                {
                    _requiredMaterial2Image.sprite = material2Data.Sprite;
                    _requiredMaterial2AmountText.text = material2Count.ToString() + "/" + craftItemInfo.ResultItem.MaterialAmounts[1].ToString();
                }
                else
                {
                    Debug.LogWarning("Material 2 data not found.");
                }
            }
            else
            {
                _material2Object.SetActive(false);
            }

            _curEquipmentAmount.text = GameManager.Instance.PlayerInventory.GetItemCount(craftItemInfo.ResultItem.Key).ToString();
        }

    }

    public void OnClickCraftBtn() //Craft-제작
    {
        Debug.Log("CraftBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        KingdomManager.Instance.ClickCraftBtn(_craftItemInfo);
    }

    private ItemData GetItemData(int key)
    {
        ItemData[] allItemData = Resources.LoadAll<ItemData>("Data/Item");

        foreach (ItemData itemData in allItemData)
        {
            if (itemData.Key == key)
            {
                return itemData;
            }
        }

        Debug.LogWarning($"ItemData with key {key} not found.");
        return null;
    }
}
