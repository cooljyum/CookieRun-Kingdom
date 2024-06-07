using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<int, CharacterData> _characterDatas = new Dictionary<int, CharacterData>();
    private Dictionary<int, BuildingData> _buildingDatas = new Dictionary<int, BuildingData>();

    //Battle
    private Dictionary<int, MonsterData> _monsterDatas = new Dictionary<int, MonsterData>();
    private Dictionary<int, MonsterGroupData> _monsterGroupDatas = new Dictionary<int, MonsterGroupData>();
    private Dictionary<int, StageData> _stageDatas = new Dictionary<int, StageData>();

    //Get
    public CharacterData GetCharacterData(int key)
    {
        if (_characterDatas.ContainsKey(key))
        {
            return _characterDatas[key];
        }
        else
        {
            Debug.LogWarning("CharacterData with key " + key + " not found.");
            return null;
        }
    }

    public BuildingData GetBuildingData(int key)
    {
        if (_buildingDatas.ContainsKey(key))
        {
            return _buildingDatas[key];
        }
        else
        {
            Debug.LogWarning("BuildingData with key " + key + " not found.");
            return null;
        }
    }

    //Battle
    public MonsterData GetMonsterData(int key)
    {
        if (_monsterDatas.ContainsKey(key))
        {
            return _monsterDatas[key];
        }
        else
        {
            Debug.LogWarning("MonsterData with key " + key + " not found.");
            return null;
        }
    }
    public MonsterGroupData GetMonsterGroupData(int key)
    {
        if (_monsterGroupDatas.ContainsKey(key))
        {
            return _monsterGroupDatas[key];
        }
        else
        {
            Debug.LogWarning("MonsterGroupData with key " + key + " not found.");
            return null;
        }
    }
    public StageData GetStageData(int key)
    {
        if (_stageDatas.ContainsKey(key))
        {
            return _stageDatas[key];
        }
        else
        {
            Debug.LogWarning("StageData with key " + key + " not found.");
            return null;
        }
    }

    //Load
    public void LoadData()
    {
        print("=== DataManager::LoadData() ===");
        LoadCookieCharacterTable();
        LoadBuildingTable();

        //Battle
        LoadBattleTables();
    }

    private void LoadCookieCharacterTable()
    {
        _characterDatas.Clear();
        CharacterData[] characterDataArray = Resources.LoadAll<CharacterData>("Data/Character");

        foreach (CharacterData data in characterDataArray)
        {
            if (!_characterDatas.ContainsKey(data.Key))
            {
                _characterDatas.Add(data.Key, data);
            }
            else
            {
                Debug.LogWarning("Duplicate CharacterData Name: " + data.Name);
            }
        }
    }

    private void LoadBuildingTable()
    {
        _buildingDatas.Clear();
        BuildingData[] buildingDataArray = Resources.LoadAll<BuildingData>("Data/Building");

        foreach (BuildingData data in buildingDataArray)
        {
            if (!_buildingDatas.ContainsKey(data.Key))
            {
                _buildingDatas.Add(data.Key, data);
            }
            else
            {
                Debug.LogWarning("Duplicate BuildingData Name: " + data.Name);
            }
        }
    }

    //Battle
    private void LoadBattleTables() 
    {
        LoadMonsterTable();
        LoadMonsterGroupTable();
        LoadStageTable();
    }
    private void LoadMonsterTable()
    {
        _monsterDatas.Clear();
        MonsterData[] monsterDataArray = Resources.LoadAll<MonsterData>("Data/Battle/Monster");

        foreach (MonsterData data in monsterDataArray)
        {
            if (!_monsterDatas.ContainsKey(data.Key))
            {
                _monsterDatas.Add(data.Key, data);
            }
            else
            {
                Debug.LogWarning("Duplicate MonsterData Name: " + data.Name);
            }
        }
    }

    private void LoadMonsterGroupTable()
    {
        _monsterGroupDatas.Clear();
        MonsterGroupData[] monsterGroupDataArray = Resources.LoadAll<MonsterGroupData>("Data/Battle/MonsterGroup");

        foreach (MonsterGroupData data in monsterGroupDataArray)
        {
            if (!_monsterGroupDatas.ContainsKey(data.Key))
            {
                _monsterGroupDatas.Add(data.Key, data);
            }
            else
            {
                Debug.LogWarning("Duplicate MonsterGroupData Name: " + data.Name);
            }
        }
    }
    private void LoadStageTable()
    {
        _stageDatas.Clear();
        StageData[] stageArray = Resources.LoadAll<StageData>("Data/Battle/Stage");

        foreach (StageData data in stageArray)
        {
            if (!_stageDatas.ContainsKey(data.Key))
            {
                _stageDatas.Add(data.Key, data);
            }
            else
            {
                Debug.LogWarning("Duplicate StageData Name: " + data.Name);
            }
        }
    }
}
