using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerDataManager _playerDataManager; //플레이어 데이터 저장 관리하는 매니저

    private string _playerDataName = "PlayerData"; //저장하는 플레이어 데이터의 json 이름

    [SerializeField]
    private PlayerData _curPlayerData; //현재 플레이어 Data

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

        // 인벤토리 초기화
        PlayerInventory = new Inventory();
        LoadPlayerInventory();
    }

    private void Start()
    {
        // 씬 변경 이벤트 구독
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        CheckAndPlayBGM();
    }

    private void CheckAndPlayBGM()
    {
        //StartScene
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            SoundManager.Instance.PlayBG("StartBgm");
        }
        //KingdomScene
        else if (SceneManager.GetActiveScene().name == "KingdomScene")
        {
            SoundManager.Instance.PlayBG("KingdomBgm");
        }
        //ReadyScene
        else if (SceneManager.GetActiveScene().name == "ReadyScene")
        {
            SoundManager.Instance.PlayBG("ReadyBgm");
        }
        //BattleScene
        else if (SceneManager.GetActiveScene().name == "BattleScene")
        {
            SoundManager.Instance.PlayBG("BattleBgm");
        }
        else
        {
            SoundManager.Instance.StopBG();
        }
    }

    private void OnActiveSceneChanged(Scene current, Scene next)
    {
        // 씬이 변경될 때 배경음악을 확인하고 재생
        CheckAndPlayBGM();
    }

    private void InitializeMyBuildings()
    {
        //BuildingData 로드
        BuildingData[] buildingDataArray = Resources.LoadAll<BuildingData>("Data/Building");

        //MyBuildings 리스트 초기화
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
        //플레이어의 인벤토리를 _curPlayerData에서 가져오기
        foreach (var item in _curPlayerData.InventoryItems)
        {
            PlayerInventory.AddItem(item.Key, item.Value);
        }
    }

    public void SavePlayerData()  //버튼 클릭과 연동하면 데이터 저장 가능 
    {
        _curPlayerData.InventoryItems = PlayerInventory.GetAllItems();
        _playerDataManager.SavePlayerData(_curPlayerData, _playerDataName);
    }

    private void OnApplicationQuit() //앱 종료시 호출
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

    public void RemoveBuilding(Building building)
    {
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
        if (MyBuildings.ContainsKey(buildingKey))
        {
            return MyBuildings[buildingKey];
        }
        return 0;
    }

    public void SaveBuilding(Building building)
    {
        CurPlayerData.BuildingKeyLists.Add(building.BuildingData.Key);
        CurPlayerData.BuildingPosLists.Add(building.transform.position);
        SavePlayerData();
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

}
