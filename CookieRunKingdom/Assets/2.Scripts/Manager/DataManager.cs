using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterData
{
    public int Key;
    public string Name;
    public int Level;
    public float Power;
}

public class DataManager : Singleton<DataManager>
{   

    private Dictionary<int, CharacterData> characterDatas = new Dictionary<int, CharacterData>();

    public CharacterData GetCharacterData(int key)
    {
        return characterDatas[key];
    }

    public void LoadData()
    {
        LoadCharacterTableExample();
    }

    private void LoadCharacterTableExample()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/CharacterDataTableExample");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string[] str = temp.Split('\n');

        for(int i = 1; i < str.Length; i++)
        {
            if (str[i].Length == 0)
                return;

            string[] data = str[i].Split(',');

            CharacterData characterData;
            characterData.Key = int.Parse(data[0]);
            characterData.Name = data[1];
            characterData.Level = int.Parse(data[2]);
            characterData.Power = float.Parse(data[3]);            

            characterDatas.Add(characterData.Key, characterData);
        }
    }
}
