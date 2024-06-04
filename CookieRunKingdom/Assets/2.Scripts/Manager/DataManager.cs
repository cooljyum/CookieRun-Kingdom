using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<int, CharacterData> _characterDatas = new Dictionary<int, CharacterData>();

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

    public void LoadData()
    {
        print("=== DataManager::LoadData() ===");
        LoadCookieCharacterTable();
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
}
