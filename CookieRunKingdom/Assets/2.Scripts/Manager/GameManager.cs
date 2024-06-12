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

        // MyBuildings ����Ʈ �ʱ�ȭ
        InitializeMyBuildings();
    }

    private void InitializeMyBuildings()
    {
        //BuildingData �ε�
        BuildingData[] buildingDataArray = Resources.LoadAll<BuildingData>("Data/Building");

        //MyBuildings ����Ʈ �ʱ�ȭ
        foreach (BuildingData buildingData in buildingDataArray)
        {
            MyBuildings.Add(buildingData.Key);
        }
    }

    private void LoadMyCards()
    {
        List<int> myCardsList = new List<int>();
        myCardsList = PlayerDataManager
    }

    public void SavePlayerData()  //��ư Ŭ���� �����ϸ� ������ ���� ���� 
    {
        _playerDataManager.SavePlayerData(_curPlayerData, _playerDataName);
    }

    private void OnApplicationQuit() //�� ����� ȣ��
    {
        SavePlayerData();
    }
}
