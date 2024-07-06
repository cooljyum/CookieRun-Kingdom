using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerDataManager _playerDataManager; // 플레이어 데이터 저장 관리하는 매니저

    private string _playerDataName = "PlayerData"; // 저장하는 플레이어 데이터의 json 이름

    [SerializeField]
    private PlayerData _curPlayerData; // 현재 플레이어 Data

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

            // 인벤토리 초기화
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
        // 씬 변경 이벤트 구독
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        CheckAndPlayBGM();
    }

    private Dictionary<int, ItemData> LoadItemData()
    {
        // ItemData 로드
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
        // 각 씬에 따른 배경음악 재생
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
        // 씬이 변경될 때 배경음악을 확인하고 재생
        CheckAndPlayBGM();
    }

    private void InitializeMyBuildings()
    {
        // BuildingData 로드
        BuildingData[] buildingDataArray = Resources.LoadAll<BuildingData>("Data/Building");

        // MyBuildings 리스트 초기화
        foreach (BuildingData buildingData in buildingDataArray)
        {
            MyBuildings[buildingData.Key] = 0;
        }
    }

    private void LoadPlayerInventory()
    {
        // 플레이어의 인벤토리를 _curPlayerData에서 가져오기
        for (int i = 0; i < _curPlayerData.InvenItemKeyLists.Count; i++)
        {
            PlayerInventory.AddItem(_curPlayerData.InvenItemKeyLists[i], _curPlayerData.InvenItemAmountLists[i]);
        }
    }

    public void SavePlayerData()
    {
        // 플레이어 데이터 저장
        var allItems = PlayerInventory.GetAllItems();
        _curPlayerData.InvenItemKeyLists = allItems.Keys.ToList();
        _curPlayerData.InvenItemAmountLists = allItems.Values.ToList();
        _playerDataManager.SavePlayerData(_curPlayerData, _playerDataName);
    }

    public void AddBuilding(int buildingKey)
    {
        // 건물 추가
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
        // 건물 제거
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
        // 건물 개수 반환
        if (MyBuildings.ContainsKey(buildingKey))
        {
            return MyBuildings[buildingKey];
        }
        return 0;
    }

    public void SaveBuilding(Building building)
    {
        // 건물 저장
        int key = building.BuildingData.Key;
        Vector2 pos = building.transform.position;

        // 중복 저장 방지
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
        // 인벤토리 레벨에 따른 최대 칸 수 계산
        switch (inventoryLevel)
        {
            case 1:
                return 32;
            case 2:
                return 64;
            case 3:
                return 128;
            default:
                return 32; // 기본 레벨 1
        }
    }

    private void OnApplicationQuit()
    {
        // 앱 종료 시 플레이어 데이터 저장
        SavePlayerData();
    }
}
