using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour  //* �κ��丮 UI *//
{
    [SerializeField]
    private Transform _inventoryContent; // �κ��丮 �����۵��� �θ� ��ü
    [SerializeField]
    private TextMeshProUGUI _sizeText; // �κ��丮 ����/��ü ĭ ��

    private GameObject _itemCellPrefab;
    private int _maxSlotCount; // �ִ� ���� ��
    private int _curSlotCount; // ���� ���� ��

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
        // �κ��丮 ������ ���� �ִ� ���� �� ���
        _maxSlotCount = GetMaxSlotsForInventoryLevel(inventoryLevel);

        // ���� ���� ���� ���� ���� Ȱ��ȭ/��Ȱ��ȭ
        for (int i = 0; i < _inventoryContent.childCount; i++)
        {
            _inventoryContent.GetChild(i).gameObject.SetActive(i < _maxSlotCount);
        }

        // ���� �� ������Ʈ
        _curSlotCount = Mathf.Min(_inventoryContent.childCount, _maxSlotCount);
        SetSizeValue();
    }

    private int GetMaxSlotsForInventoryLevel(int inventoryLevel) // �κ� ������ ���� �ִ� ���� �� ��ȯ
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
        // �÷��̾� ������ �� �κ��丮 ������ ����Ͽ� ���� ����
        int inventoryLevel = GameManager.Instance.CurPlayerData.InventoryLevel;
        _maxSlotCount = GetMaxSlotsForInventoryLevel(inventoryLevel);

        // ��� �������� ������
        var allItems = GameManager.Instance.PlayerInventory.GetAllItems();
        int slotIndex = 0;

        // ���� ���� ����
        foreach (Transform child in _inventoryContent)
        {
            Destroy(child.gameObject);
        }

        // �� �������� ���Կ� ǥ��
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

                // ���� ����
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

        // ���� �� ������Ʈ
        _curSlotCount = slotIndex;
        SetSizeValue();
    }

    private void CreateItemSlot(Enums.ItemType type) // Ÿ�Կ� �´� ������ ���Ը� Ȱ��ȭ
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

    private void SetSizeValue() // ĭ �� �� ����
    {
        _sizeText.text = $"{_curSlotCount}/{_maxSlotCount}";
    }

    public void OnClickExitBtn()  // Inven-������
    {
        Debug.Log("ExitBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        gameObject.SetActive(false);
    }

    public void OnClickAllBtn() // Inven-��ü
    {
        Debug.Log("AllBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        LoadPlayerInventory();
    }

    public void OnClickConsumablesBtn() // Inven-�Ҹ�ǰ
    {
        Debug.Log("ConsumablesBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        CreateItemSlot(Enums.ItemType.Consumable);
    }

    public void OnClickUsableBtn() // Inven-��� ����
    {
        Debug.Log("UsableBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        CreateItemSlot(Enums.ItemType.Usable);
    }

    public void OnClickRareMaterialBtn() // Inven-������
    {
        Debug.Log("RareMaterialBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        CreateItemSlot(Enums.ItemType.RareMaterial);
    }

    public void OnClickProductBtn() // Inven-����ǰ
    {
        Debug.Log("ProductBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        CreateItemSlot(Enums.ItemType.Product);
    }

    public void OnClickETCBtn() // Inven-��Ÿ
    {
        Debug.Log("ETCBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        CreateItemSlot(Enums.ItemType.ETC);
    }

    public void OnClickExtendBtn() // Inven-ũ�� Ȯ��
    {
        Debug.Log("ExtendBtn Click");
        SoundManager.Instance.PlayFX("BtnClick");
        // Ȯ��UI.gameObject.SetActive(true);
    }
}