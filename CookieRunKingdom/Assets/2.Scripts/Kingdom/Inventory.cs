using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Enums;

public class Inventory
{
    private Dictionary<int, List<int>> _items; // ������ Ű�� ���� ����Ʈ ���� ����
    private Dictionary<int, ItemData> _itemData; // ������ Ű�� ������ ������ ���� ����
    private Dictionary<ItemType, int> _maxItemAmountPerSlot; // ������ Ÿ�Ժ� �ִ� ����

    // ������ �ʱ�ȭ
    public Inventory(Dictionary<int, ItemData> itemData)
    {
        _items = new Dictionary<int, List<int>>();
        _itemData = itemData;
        _maxItemAmountPerSlot = new Dictionary<ItemType, int>
        {
            { ItemType.Product, 50 },
            { ItemType.Consumable, 99 },
            { ItemType.Usable, 999 },
            { ItemType.RareMaterial, 99 },
            { ItemType.ETC, 999 }
        };
    }

    public void AddItem(int itemKey, int count) // ������ �߰�
    {
        ItemType itemType = _itemData[itemKey].Type;
        int maxAmountPerSlot = GetMaxItemAmountPerSlot(itemType);

        if (!_items.ContainsKey(itemKey))
        {
            _items[itemKey] = new List<int>();
        }

        while (count > 0)
        {
            int remainingSpace = maxAmountPerSlot;

            if (_items[itemKey].Count > 0)
            {
                int lastSlotAmount = _items[itemKey].Last();
                remainingSpace = maxAmountPerSlot - lastSlotAmount;

                if (remainingSpace > 0)
                {
                    int addAmount = Mathf.Min(count, remainingSpace);
                    _items[itemKey][_items[itemKey].Count - 1] += addAmount;
                    count -= addAmount;
                    continue;
                }
            }

            int newSlotAmount = Mathf.Min(count, maxAmountPerSlot);
            _items[itemKey].Add(newSlotAmount);
            count -= newSlotAmount;
        }
    }

    public int GetItemAmount(int itemKey) // key�� �´� �������� ���� ��ȯ
    {
        if (_items.ContainsKey(itemKey))
        {
            return _items[itemKey].Sum(); // �������� �����ϸ� ���� �հ� ��ȯ
        }
        return 0; // �������� ������ 0 ��ȯ
    }

    public void RemoveItem(int itemKey, int count) // ������ ����
    {
        if (_items.ContainsKey(itemKey))
        {
            while (count > 0 && _items[itemKey].Count > 0)
            {
                int firstSlotAmount = _items[itemKey].First();
                int removeAmount = Mathf.Min(count, firstSlotAmount);
                _items[itemKey][0] -= removeAmount;
                count -= removeAmount;

                if (_items[itemKey][0] <= 0)
                {
                    _items[itemKey].RemoveAt(0);
                }
            }

            if (_items[itemKey].Count == 0)
            {
                _items.Remove(itemKey); // ������ 0 ���ϰ� �Ǹ� Dictionary���� ����
            }
        }
    }

    public Dictionary<int, List<int>> GetAllItems() // ��� ������ ��ȯ
    {
        return new Dictionary<int, List<int>>(_items); // Dictionary�� ���纻 ��ȯ
    }

    public Dictionary<int, List<int>> GetItemsByType(Enums.ItemType type) // Ư�� Ÿ���� ������ ��ȯ
    {
        if (type == Enums.ItemType.All)
        {
            return GetAllItems(); // Ÿ���� 'All'�̸� ��� ������ ��ȯ
        }

        // Ư�� Ÿ���� ������ ���͸��Ͽ� ��ȯ
        return _items
            .Where(item => _itemData[item.Key].Type == type)
            .ToDictionary(item => item.Key, item => new List<int>(item.Value));
    }

    public int GetMaxItemAmountPerSlot(ItemType type) // ������ Ÿ�Կ� ���� �ִ� ���� ��ȯ
    {
        if (_maxItemAmountPerSlot.ContainsKey(type))
        {
            return _maxItemAmountPerSlot[type];
        }
        return int.MaxValue; // �⺻��
    }
}
