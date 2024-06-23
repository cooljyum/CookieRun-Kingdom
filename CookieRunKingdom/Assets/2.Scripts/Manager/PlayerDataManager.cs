using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    private PlayerData _playerData;

    // playerData: ������ �÷��̾� ������ ��ü 
    // jsonName: ������ JSON ���� �̸� (Ȯ���� ����)
    public void SavePlayerData(PlayerData playerData, string jsonName)
    {
        //string savePath = Path.Combine(Application.persistentDataPath, jsonName +".json"); //����(�ش� �ڱ��ڽ� ��ǻ��) //"C:\Users\<������̸�>\AppData\LocalLow\<ȸ���̸�>\<������Ʈ�̸�>" ������ ����
        string savePath = Path.Combine(Application.dataPath, "DataJson", jsonName +".json"); //������Ʈ �ȿ� ���� //"Assets/DataJson" ������ ����
        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText(savePath, json);
        print("Success Save PlayData! dataName:"+ jsonName + "/Path : " + savePath);
    }

    // jsonName: ������ JSON ���� �̸� (Ȯ���� ����)
    // ��ȯ��: �ε�� �÷��̾� ������ ��ü
    public PlayerData LoadPlayerData(string jsonName)
    {
        //string loadPath = Path.Combine(Application.persistentDataPath, jsonName +".json"); //����(�ش� �ڱ��ڽ� ��ǻ��) //"C:\Users\<������̸�>\AppData\LocalLow\<ȸ���̸�>\<������Ʈ�̸�>" ������ ����
        string loadPath = Path.Combine("Assets/DataJson", jsonName + ".json"); //������Ʈ �ȿ� ���� //"Assets/DataJson" ������ ����
        if (File.Exists(loadPath))
        {
            string json = File.ReadAllText(loadPath);
            _playerData = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            // ������ �������� ������ �⺻ ������ ����
            _playerData = CreateDefaultPlayerData();
        }

        print("Success Load PlayData! dataName:" + jsonName + "/Path : " + loadPath);
        return _playerData;
    }

    // �⺻ �÷��̾� �����͸� ����
    // ��ȯ��: �⺻ �÷��̾� ������ ��ü
    private PlayerData CreateDefaultPlayerData()
    {
        PlayerData playerData = new PlayerData();
        playerData.Level = 1;
        playerData.Exp = 0;
        playerData.Coin = 10000;
        playerData.Mileage = 0;
        playerData.CurStage = 1;
        playerData.MyCardsLists = new System.Collections.Generic.List<int>();
        playerData.DeckKeyLists = new System.Collections.Generic.List<int>();
        playerData.BuildingKeyLists = new System.Collections.Generic.List<int>();
        playerData.PosIndexLists = new System.Collections.Generic.List<List<int>>();
        playerData.InvenItemKeyLists = new System.Collections.Generic.List<int>();
        playerData.InvenItemAmountLists = new System.Collections.Generic.List<int>();
        return playerData;
    }
}
