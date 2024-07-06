using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Enums;

public class Inventory
{
    private Dictionary<int, int> _items; // 아이템 키와 수량 저장 공간
    private Dictionary<int, ItemData> _itemData; // 아이템 키와 아이템 데이터 저장 공간
    private Dictionary<ItemType, int> _maxItemAmountPerSlot; // 아이템 타입별 최대 개수

    // 생성자 초기화
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

    public void AddItem(int itemKey, int count) // 아이템 추가
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

    public int GetItemAmount(int itemKey) // key에 맞는 아이템의 수량 반환
    {
        if (_items.ContainsKey(itemKey))
        {
            return _items[itemKey]; // 아이템이 존재하면 수량 반환
        }
        return 0; // 아이템이 없으면 0 반환
    }

    public void RemoveItem(int itemKey, int count) // 아이템 삭제
    {
        if (_items.ContainsKey(itemKey))
        {
            _items[itemKey] -= count; // 아이템 수량 감소
            if (_items[itemKey] <= 0)
            {
                _items.Remove(itemKey); // 수량이 0 이하가 되면 Dictionary에서 제거
            }
        }
    }

    public Dictionary<int, int> GetAllItems() // 모든 아이템 반환
    {
        return new Dictionary<int, int>(_items); // Dictionary의 복사본 반환
    }

    public Dictionary<int, int> GetItemsByType(Enums.ItemType type) // 특정 타입의 아이템 반환
    {
        if (type == Enums.ItemType.All)
        {
            return GetAllItems(); // 타입이 'All'이면 모든 아이템 반환
        }

        // 특정 타입의 아이템 필터링하여 반환
        return _items
            .Where(item => _itemData[item.Key].Type == type)
            .ToDictionary(item => item.Key, item => item.Value);
    }

    public int GetMaxItemAmountPerSlot(ItemType type) // 아이템 타입에 따른 최대 개수 반환
    {
        if (_maxItemAmountPerSlot.ContainsKey(type))
        {
            return _maxItemAmountPerSlot[type];
        }
        return int.MaxValue; // 기본값
    }
}
