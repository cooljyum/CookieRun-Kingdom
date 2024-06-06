using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<int, CharacterData> _characterDatas = new Dictionary<int, CharacterData>();
    private Dictionary<int, BuildingData> _buildingDatas = new Dictionary<int, BuildingData>();

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

    public void LoadData()
    {
        print("=== DataManager::LoadData() ===");
        LoadCookieCharacterTable();
        LoadBuildingTable();
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
}
