using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerDataManager _playerDataManager; //�÷��̾� ������ ���� �����ϴ� �Ŵ���

    private string _playerDataName = "PlayerData"; //�����ϴ� �÷��̾� �������� json �̸�

    [SerializeField]
    private PlayerData _curPlayerData; //���� �÷��̾� Data

    public PlayerData CurPlayerData 
    {
        get { return _curPlayerData; }
        set { _curPlayerData = value; }
    }

    public Dictionary<int, int> MyBuildings = new Dictionary<int, int>();
    public Inventory PlayerInventory { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);

        // Load Manager 
        DataManager.Instance.LoadData();

        _playerDataManager = new PlayerDataManager();
        _curPlayerData = _playerDataManager.LoadPlayerData(_playerDataName);

        InitializeMyBuildings();

        // �κ��丮 �ʱ�ȭ
        PlayerInventory = new Inventory();
        LoadPlayerInventory();
    }

    private void InitializeMyBuildings()
    {
        //BuildingData �ε�
        BuildingData[] buildingDataArray = Resources.LoadAll<BuildingData>("Data/Building");

        //MyBuildings ����Ʈ �ʱ�ȭ
        foreach (BuildingData buildingData in buildingDataArray)
        {
            MyBuildings[buildingData.Key] = 0;
        }
    }

    private void LoadMyCards()
    {
        //List<int> myCardsList = new List<int>();
        //myCardsList = PlayerDataManager
    }

    private void LoadPlayerInventory()
    {
        //�÷��̾��� �κ��丮�� _curPlayerData���� ��������
        foreach (var item in _curPlayerData.InventoryItems)
        {
            PlayerInventory.AddItem(item.Key, item.Value);
        }
    }

    public void SavePlayerData()  //��ư Ŭ���� �����ϸ� ������ ���� ���� 
    {
        _curPlayerData.InventoryItems = PlayerInventory.GetAllItems();
        _playerDataManager.SavePlayerData(_curPlayerData, _playerDataName);
    }

    private void OnApplicationQuit() //�� ����� ȣ��
    {
        //SavePlayerData();
    }

    public void AddBuilding(int buildingKey)
    {
        if (MyBuildings.ContainsKey(buildingKey))
        {
            MyBuildings[buildingKey]++;
        }
        else
        {
            MyBuildings[buildingKey] = 1;
        }
    }

    public int GetBuildingCount(int buildingKey)
    {
        if (MyBuildings.ContainsKey(buildingKey))
        {
            return MyBuildings[buildingKey];
        }
        return 0;
    }
}
