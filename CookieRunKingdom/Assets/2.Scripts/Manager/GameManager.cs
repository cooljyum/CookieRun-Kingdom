using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerDataManager _playerDataManager; // �÷��̾� ������ ���� �����ϴ� �Ŵ���

    private string _playerDataName = "PlayerData"; // �����ϴ� �÷��̾� �������� json �̸�

    [SerializeField]
    private PlayerData _curPlayerData; // ���� �÷��̾� Data

    public PlayerData CurPlayerData
    {
        get { return _curPlayerData; }
        set { _curPlayerData = value; }
    }

    public Dictionary<int, int> MyBuildings = new Dictionary<int, int>();
    public Inventory PlayerInventory { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

            // Load Manager 
            DataManager.Instance.LoadData();

            _playerDataManager = new PlayerDataManager();
            _curPlayerData = _playerDataManager.LoadPlayerData(_playerDataName);

            InitializeMyBuildings();

            // �κ��丮 �ʱ�ȭ
            PlayerInventory = new Inventory(LoadItemData());
            LoadPlayerInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // �� ���� �̺�Ʈ ����
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        CheckAndPlayBGM();
    }

    private Dictionary<int, ItemData> LoadItemData()
    {
        // ItemData �ε�
        ItemData[] itemDataArray = Resources.LoadAll<ItemData>("Data/Items");
        Dictionary<int, ItemData> itemDataDictionary = new Dictionary<int, ItemData>();

        foreach (ItemData itemData in itemDataArray)
        {
            itemDataDictionary[itemData.Key] = itemData;
        }

        return itemDataDictionary;
    }

    private void CheckAndPlayBGM()
    {
        // �� ���� ���� ������� ���
        switch (SceneManager.GetActiveScene().name)
        {
            case "StartScene":
                SoundManager.Instance.PlayBG("StartBgm");
                break;
            case "KingdomScene":
                SoundManager.Instance.PlayBG("KingdomBgm");
                break;
            case "ReadyScene":
                SoundManager.Instance.PlayBG("ReadyBgm");
                break;
            case "BattleScene":
                SoundManager.Instance.PlayBG("BattleBgm");
                break;
            case "GachaScene":
                SoundManager.Instance.PlayBG("GachaBgm");
                break;
            default:
                SoundManager.Instance.StopBG();
                break;
        }
    }

    private void OnActiveSceneChanged(Scene current, Scene next)
    {
        // ���� ����� �� ��������� Ȯ���ϰ� ���
        CheckAndPlayBGM();
    }

    private void InitializeMyBuildings()
    {
        // BuildingData �ε�
        BuildingData[] buildingDataArray = Resources.LoadAll<BuildingData>("Data/Building");

        // MyBuildings ����Ʈ �ʱ�ȭ
        foreach (BuildingData buildingData in buildingDataArray)
        {
            MyBuildings[buildingData.Key] = 0;
        }
    }

    private void LoadPlayerInventory()
    {
        // �÷��̾��� �κ��丮�� _curPlayerData���� ��������
        for (int i = 0; i < _curPlayerData.InvenItemKeyLists.Count; i++)
        {
            PlayerInventory.AddItem(_curPlayerData.InvenItemKeyLists[i], _curPlayerData.InvenItemAmountLists[i]);
        }
    }

    public void SavePlayerData()
    {
        // �÷��̾� ������ ����
        var allItems = PlayerInventory.GetAllItems();
        _curPlayerData.InvenItemKeyLists = allItems.Keys.ToList();
        _curPlayerData.InvenItemAmountLists = allItems.Values.ToList();
        _playerDataManager.SavePlayerData(_curPlayerData, _playerDataName);
    }

    public void AddBuilding(int buildingKey)
    {
        // �ǹ� �߰�
        if (MyBuildings.ContainsKey(buildingKey))
        {
            MyBuildings[buildingKey]++;
        }
        else
        {
            MyBuildings[buildingKey] = 1;
        }
    }

    public void RemoveBuilding(Building building)
    {
        // �ǹ� ����
        int index = CurPlayerData.BuildingKeyLists.IndexOf(building.BuildingData.Key);
        if (index != -1)
        {
            CurPlayerData.BuildingKeyLists.RemoveAt(index);
            CurPlayerData.BuildingPosLists.RemoveAt(index);
            SavePlayerData();
        }
    }

    public int GetBuildingCount(int buildingKey)
    {
        // �ǹ� ���� ��ȯ
        if (MyBuildings.ContainsKey(buildingKey))
        {
            return MyBuildings[buildingKey];
        }
        return 0;
    }

    public void SaveBuilding(Building building)
    {
        // �ǹ� ����
        int key = building.BuildingData.Key;
        Vector2 pos = building.transform.position;

        // �ߺ� ���� ����
        if (!CurPlayerData.BuildingKeyLists.Contains(key) || !CurPlayerData.BuildingPosLists.Contains(pos))
        {
            CurPlayerData.BuildingKeyLists.Add(key);
            CurPlayerData.BuildingPosLists.Add(pos);
            SavePlayerData();
        }
    }

    public List<(int, Vector2)> LoadBuildings()
    {
        List<(int, Vector2)> buildings = new List<(int, Vector2)>();
        for (int i = 0; i < CurPlayerData.BuildingKeyLists.Count; i++)
        {
            buildings.Add((CurPlayerData.BuildingKeyLists[i], CurPlayerData.BuildingPosLists[i]));
        }
        return buildings;
    }

    private int GetMaxInventorySlots(int inventoryLevel)
    {
        // �κ��丮 ������ ���� �ִ� ĭ �� ���
        switch (inventoryLevel)
        {
            case 1:
                return 32;
            case 2:
                return 64;
            case 3:
                return 128;
            default:
                return 32; // �⺻ ���� 1
        }
    }

    private void OnApplicationQuit()
    {
        // �� ���� �� �÷��̾� ������ ����
        SavePlayerData();
    }
}
