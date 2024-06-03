using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : BaseUI
{
    [SerializeField] private ItemData _emptyItemData;
    [SerializeField] private ItemSlot itemSlot;
    [SerializeField] private RectTransform itemSlotParent;

    public int maxSlot = 80;

    private List<ItemSlot> inventoryItems = new List<ItemSlot>();

    private KingdomManager _manager;

    public override void Hide()
    {
        base.Hide();
        _manager.IsMoveCamera = true;
    }

    public override void Show()
    {
        base.Show();
        _manager.IsMoveCamera = false;
        UpdateInventory();
    }

    public void ExpandCapacity(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            ItemSlot slot = Instantiate(itemSlot, itemSlotParent);
            inventoryItems.Add(slot);
            slot.ClearSlot();
        }
    }

    public void ExitUI()
    {
        GameManager.UI.ExitPopUpUI();
    }

    public override void Init()
    {
        base.Init();

        _manager = FindObjectOfType<KingdomManager>();

        itemSlotParent.sizeDelta = new Vector2(itemSlotParent.sizeDelta.x, 30 + (165 + 30) * (maxSlot / 8));

        // √ ±‚»≠
        for (int i = 0; i < maxSlot; i++)
        {
            ItemSlot slot = Instantiate(itemSlot, itemSlotParent);
            inventoryItems.Add(slot);
            inventoryItems[i].FillSlot(_emptyItemData, 0);
            slot.ClearSlot();
        }
    }

    private void UpdateInventory()
    {
        int index = 0;
        List<ItemSlot> itemList = new List<ItemSlot>();
        foreach (KeyValuePair<ItemData, int> data in DataBaseManager.Instance.MyDataBase.itemDataBase)
        {
            if(data.Value != 0)
            {
                itemList.Add(inventoryItems[index]);
                inventoryItems[index++].FillSlot(data.Key, data.Value);
            }
        }

        itemList.Sort((item1, item2) => (item1.Data.ItemType).CompareTo(item2.Data.ItemType));

        for(int i = 0; i < itemList.Count; i++)
            itemList[i].transform.SetSiblingIndex(i);
    }
}
