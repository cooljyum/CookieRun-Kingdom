using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
        // �� ���� �̺�Ʈ ����
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
        else if(SceneManager.GetActiveScene().name == "GachaScene")
        {
            SoundManager.Instance.PlayBG("GachaBgm");
        }
        else
        {
            SoundManager.Instance.StopBG();
        }
    }

    private void OnActiveSceneChanged(Scene current, Scene next)
    {
        // ���� ����� �� ��������� Ȯ���ϰ� ���
        CheckAndPlayBGM();
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
        // �÷��̾��� �κ��丮�� _curPlayerData���� ��������
        for (int i = 0; i < _curPlayerData.InvenItemKeyLists.Count; i++)
        {
            PlayerInventory.AddItem(_curPlayerData.InvenItemKeyLists[i], _curPlayerData.InvenItemAmountLists[i]);
        }
    }

    public void SavePlayerData()  //��ư Ŭ���� �����ϸ� ������ ���� ���� 
    {
        _curPlayerData.InvenItemKeyLists = PlayerInventory.GetAllItems().Keys.ToList();
        _curPlayerData.InvenItemAmountLists = PlayerInventory.GetAllItems().Values.ToList();
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
}
