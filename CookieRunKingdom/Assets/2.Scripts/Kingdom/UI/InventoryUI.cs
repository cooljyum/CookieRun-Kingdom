using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour  //* 인벤토리 UI *//
{
    [SerializeField]
    private Transform _inventoryContent; // 인벤토리 아이템들의 부모 객체
    [SerializeField]
    private TextMeshProUGUI _sizeText; // 인벤토리 현재/전체 칸 수

    private GameObject _itemCellPrefab;
    private int _maxSlotCount; // 최대 슬롯 수
    private int _curSlotCount; // 현재 슬롯 수

    private void Awake()
    {
        _itemCellPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/ETC/InvenItemCell");
    }

    private void Start()
    {
        SetSizeValue();
    }

    public void SetInventorySlots(int inventoryLevel)
    {
        // 인벤토리 레벨에 따른 최대 슬롯 수 계산
        _maxSlotCount = GetMaxSlotsForInventoryLevel(inventoryLevel);

        // 현재 슬롯 수에 따라 슬롯 활성화/비활성화
        for (int i = 0; i < _inventoryContent.childCount; i++)
        {
            _inventoryContent.GetChild(i).gameObject.SetActive(i < _maxSlotCount);
        }

        // 슬롯 수 업데이트
        _curSlotCount = Mathf.Min(_inventoryContent.childCount, _maxSlotCount);
        SetSizeValue();
    }

    private int GetMaxSlotsForInventoryLevel(int inventoryLevel) // 인벤 레벨에 따라 최대 슬롯 수 반환
    {
        return inventoryLevel switch
        {
            1 => 30,
            2 => 50,
            3 => 70,
            _ => 30,
        };
    }

    public void LoadPlayerInventory()
    {
        // 플레이어 데이터 속 인벤토리 레벨을 사용하여 슬롯 세팅
        int inventoryLevel = GameManager.Instance.CurPlayerData.InventoryLevel;
        _maxSlotCount = GetMaxSlotsForInventoryLevel(inventoryLevel);

        // 모든 아이템을 가져옴
        var allItems = GameManager.Instance.PlayerInventory.GetAllItems();
        int slotIndex = 0;

        // 기존 슬롯 제거
        foreach (Transform child in _inventoryContent)
        {
            Destroy(child.gameObject);
        }

        // 각 아이템을 슬롯에 표시
        foreach (var item in allItems)
        {
            if (slotIndex >= _maxSlotCount) break;

            var itemData = DataManager.Instance.GetItemData(item.Key);
            if (itemData == null)
            {
                Debug.LogError($"ItemData for key {item.Key} is null.");
                continue;
            }

            foreach (var amount in item.Value)
            {
                if (slotIndex >= _maxSlotCount) break;

                // 슬롯 생성
                var itemSlotObject = Instantiate(_itemCellPrefab, _inventoryContent);
                var itemSlot = itemSlotObject.GetComponent<InventoryItemUI>();

                if (itemSlot == null)
                {
                    Debug.LogError("ItemSlot is null.");
                    continue;
                }

                itemSlot.SetInventorySlotData(itemData, amount);
                slotIndex++;
            }
        }

        // 슬롯 수 업데이트
        _curSlotCount = slotIndex;
        SetSizeValue();
    }

    private void CreateItemSlot(Enums.ItemType type) // 타입에 맞는 아이템 슬롯만 활성화
    {
        foreach (Transform child in _inventoryContent)
        {
            var itemSlot = child.GetComponent<InventoryItemUI>();
            if (itemSlot.gameObject.activeSelf)
            {
                var itemData = itemSlot.GetItemData();
                if (itemData != null && itemData.Type == type)
                    itemSlot.gameObject.SetActive(true);
                else
                    itemSlot.gameObject.SetActive(false);
            }
        }
    }

    private void SetSizeValue() // 칸 수 값 세팅
    {
        _sizeText.text = $"{_curSlotCount}/{_maxSlotCount}";
    }

    public void OnClickExitBtn()  // Inven-나가기
    {
        Debug.Log("ExitBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        gameObject.SetActive(false);
    }

    public void OnClickAllBtn() // Inven-전체
    {
        Debug.Log("AllBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        LoadPlayerInventory();
    }

    public void OnClickConsumablesBtn() // Inven-소모품
    {
        Debug.Log("ConsumablesBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        CreateItemSlot(Enums.ItemType.Consumable);
    }

    public void OnClickUsableBtn() // Inven-사용 가능
    {
        Debug.Log("UsableBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        CreateItemSlot(Enums.ItemType.Usable);
    }

    public void OnClickRareMaterialBtn() // Inven-희귀재료
    {
        Debug.Log("RareMaterialBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        CreateItemSlot(Enums.ItemType.RareMaterial);
    }

    public void OnClickProductBtn() // Inven-생산품
    {
        Debug.Log("ProductBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        CreateItemSlot(Enums.ItemType.Product);
    }

    public void OnClickETCBtn() // Inven-기타
    {
        Debug.Log("ETCBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        CreateItemSlot(Enums.ItemType.ETC);
    }

    public void OnClickExtendBtn() // Inven-크기 확장
    {
        Debug.Log("ExtendBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        // 확장UI.gameObject.SetActive(true);
    }
}