using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    private PlayerData _playerData;

    // playerData: 저장할 플레이어 데이터 객체 
    // jsonName: 저장할 JSON 파일 이름 (확장자 제외)
    public void SavePlayerData(PlayerData playerData, string jsonName)
    {
        //string savePath = Path.Combine(Application.persistentDataPath, jsonName +".json"); //로컬(해당 자기자신 컴퓨터) //"C:\Users\<사용자이름>\AppData\LocalLow\<회사이름>\<프로젝트이름>" 폴더에 저장
        string savePath = Path.Combine(Application.dataPath, "DataJson", jsonName +".json"); //프로젝트 안에 저장 //"Assets/DataJson" 폴더에 저장
        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText(savePath, json);
        print("Success Save PlayData! dataName:"+ jsonName + "/Path : " + savePath);
    }

    // jsonName: 저장할 JSON 파일 이름 (확장자 제외)
    // 반환값: 로드된 플레이어 데이터 객체
    public PlayerData LoadPlayerData(string jsonName)
    {
        //string loadPath = Path.Combine(Application.persistentDataPath, jsonName +".json"); //로컬(해당 자기자신 컴퓨터) //"C:\Users\<사용자이름>\AppData\LocalLow\<회사이름>\<프로젝트이름>" 폴더에 저장
        string loadPath = Path.Combine("Assets/DataJson", jsonName + ".json"); //프로젝트 안에 저장 //"Assets/DataJson" 폴더에 저장
        if (File.Exists(loadPath))
        {
            string json = File.ReadAllText(loadPath);
            _playerData = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            // 파일이 존재하지 않으면 기본 데이터 생성
            _playerData = CreateDefaultPlayerData();
        }

        print("Success Load PlayData! dataName:" + jsonName + "/Path : " + loadPath);
        return _playerData;
    }

    // 기본 플레이어 데이터를 생성
    // 반환값: 기본 플레이어 데이터 객체
    private PlayerData CreateDefaultPlayerData()
    {
        PlayerData playerData = new PlayerData();
        playerData.Level = 1;
        playerData.Exp = 0;
        playerData.Coin = 100000;
        playerData.Diamond = 10000;
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
