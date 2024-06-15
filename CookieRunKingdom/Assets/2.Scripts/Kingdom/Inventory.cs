using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<int, int> _items;

    public Inventory()
    {
        _items = new Dictionary<int, int>();
    }

    //������ �߰�
    public void AddItem(int itemKey, int count)
    {
        if (_items.ContainsKey(itemKey))
        {
            _items[itemKey] += count;
        }
        else
        {
            _items[itemKey] = count;
        }
    }

    public int GetItemCount(int itemKey)
    {
        if (_items.ContainsKey(itemKey))
        {
            return _items[itemKey];
        }
        return 0;
    }

    //������ ����
    public void RemoveItem(int itemKey, int count)
    {
        if (_items.ContainsKey(itemKey))
        {
            _items[itemKey] -= count;
            if (_items[itemKey] <= 0)
            {
                _items.Remove(itemKey);
            }
        }
    }

    //��� ������ ��������
    public Dictionary<int, int> GetAllItems()
    {
        return new Dictionary<int, int>(_items);
    }
}
