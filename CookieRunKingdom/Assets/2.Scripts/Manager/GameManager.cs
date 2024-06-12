using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

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

    public List<int> MyCards = new List<int>();
    public List<int> MyBuildings = new List<int>();
    public List<int> MyItems = new List<int>();

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);

        // Load Manager 
        DataManager.Instance.LoadData();

        _playerDataManager = new PlayerDataManager();
        _curPlayerData = _playerDataManager.LoadPlayerData(_playerDataName);

        // MyBuildings 리스트 초기화
        InitializeMyBuildings();
    }

    private void InitializeMyBuildings()
    {
        //BuildingData 로드
        BuildingData[] buildingDataArray = Resources.LoadAll<BuildingData>("Data/Building");

        //MyBuildings 리스트 초기화
        foreach (BuildingData buildingData in buildingDataArray)
        {
            MyBuildings.Add(buildingData.Key);
        }
    }

    public void SavePlayerData()  //버튼 클릭과 연동하면 데이터 저장 가능 
    {
        _playerDataManager.SavePlayerData(_curPlayerData, _playerDataName);
    }

    private void OnApplicationQuit() //앱 종료시 호출
    {
        SavePlayerData();
    }
}
