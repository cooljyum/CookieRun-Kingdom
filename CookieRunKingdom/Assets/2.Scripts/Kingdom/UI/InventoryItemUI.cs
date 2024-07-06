using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour  //* �κ��丮 â ������ UI *//
{
    [SerializeField]
    private Image _slotImage; //���� ��� �̹���
    [SerializeField]
    private GameObject _cellParent; //������ ���� �� ��Ȱ��ȭ�� �θ� ������Ʈ
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
