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

    public List<int> myCards = new List<int>();

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);

        // Load Manager 
        DataManager.Instance.LoadData();

        _playerDataManager = new PlayerDataManager();
        _curPlayerData = _playerDataManager.LoadPlayerData(_playerDataName);
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
