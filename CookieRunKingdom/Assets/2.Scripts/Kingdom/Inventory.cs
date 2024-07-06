using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Enums;

public class Inventory
{
    private Dictionary<int, int> _items; // ������ Ű�� ���� ���� ����
    private Dictionary<int, ItemData> _itemData; // ������ Ű�� ������ ������ ���� ����
    private Dictionary<ItemType, int> _maxItemAmountPerSlot; // ������ Ÿ�Ժ� �ִ� ����

    // ������ �ʱ�ȭ
    public Inventory(Dictionary<int, ItemData> itemData)
    {
        _items = new Dictionary<int, int>();
        _itemData = itemData;
        _maxItemAmountPerSlot = new Dictionary<ItemType, int>
        {
            { ItemType.Product, 50 },
            { ItemType.Consumable, 999 }, 
            { ItemType.Usable, 999 }, 
            { ItemType.RareMaterial, 99 },
            { ItemType.ETC, 999 } 
        };
    }

    public void AddItem(int itemKey, int count) // ������ �߰�
    {
        ItemType itemType = _itemData[itemKey].Type;
        int maxAmount = _maxItemAmountPerSlot.ContainsKey(itemType) ? _maxItemAmountPerSlot[itemType] : int.MaxValue;

        if (_items.ContainsKey(itemKey))
        {
            _items[itemKey] = Mathf.Min(_items[itemKey] + count, maxAmount);
        }
        else
        {
            _items[itemKey] = Mathf.Min(count, maxAmount);
        }
    }

    public int GetItemAmount(int itemKey) // key�� �´� �������� ���� ��ȯ
    {
        if (_items.ContainsKey(itemKey))
        {
            return _items[itemKey]; // �������� �����ϸ� ���� ��ȯ
        }
        return 0; // �������� ������ 0 ��ȯ
    }

    public void RemoveItem(int itemKey, int count) // ������ ����
    {
        if (_items.ContainsKey(itemKey))
        {
            _items[itemKey] -= count; // ������ ���� ����
            if (_items[itemKey] <= 0)
            {
                _items.Remove(itemKey); // ������ 0 ���ϰ� �Ǹ� Dictionary���� ����
            }
        }
    }

    public Dictionary<int, int> GetAllItems() // ��� ������ ��ȯ
    {
        return new Dictionary<int, int>(_items); // Dictionary�� ���纻 ��ȯ
    }

    public Dictionary<int, int> GetItemsByType(Enums.ItemType type) // Ư�� Ÿ���� ������ ��ȯ
    {
        if (type == Enums.ItemType.All)
        {
            return GetAllItems(); // Ÿ���� 'All'�̸� ��� ������ ��ȯ
        }

        // Ư�� Ÿ���� ������ ���͸��Ͽ� ��ȯ
        return _items
            .Where(item => _itemData[item.Key].Type == type)
            .ToDictionary(item => item.Key, item => item.Value);
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
