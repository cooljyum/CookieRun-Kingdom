using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour  //* 인벤토리 창 아이템 UI *//
{
    [SerializeField]
    private Image _slotImage; //슬롯 배경 이미지
    [SerializeField]
    private GameObject _cellParent; //데이터 없을 때 비활성화용 부모 오브젝트
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private TextMeshProUGUI _amountText;

    private ItemData _itemData;

    public void SetInventorySlotData(ItemData itemData, int amount)
    {
        _itemData = itemData;
        _itemImage.sprite = itemData.Sprite;
        _amountText.text = amount.ToString();
        _cellParent.SetActive(true);
    }

    public ItemData GetItemData()
    {
        return _itemData;
    }
}
