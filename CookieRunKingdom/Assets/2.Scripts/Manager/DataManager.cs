using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterData
{
    public int Key;
    public string Name;
    public int Level;
    public float Hp;
    public float Attack;
    public float Defence;
    public float Critical;
    public int Type;
    public int Position;
    public int Rarity;
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
        LoadCookieCharacterTable();
    }

    private void LoadCookieCharacterTable()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/CookieCharacterTable");

        string temp = textAsset.text.Replace("\r\n", "\n");

        string[] str = temp.Split('\n');

        print("===== DataManager::LoadCookieCharacterTable() =====");

        for (int i = 1; i < str.Length; i++)
        {
            if (str[i].Length == 0)
                return;

            string[] data = str[i].Split(',');

            CharacterData characterData;
            characterData.Key = int.Parse(data[0]);
            characterData.Name = data[1];
            characterData.Level = int.Parse(data[2]);
            characterData.Hp = float.Parse(data[3]);            
            characterData.Attack = float.Parse(data[4]);            
            characterData.Defence = float.Parse(data[5]);            
            characterData.Critical = float.Parse(data[6]);            
            characterData.Type = int.Parse(data[7]);            
            characterData.Position = int.Parse(data[8]);            
            characterData.Rarity = int.Parse(data[9]);            

            characterDatas.Add(characterData.Key, characterData);

            //PrintData
            print("{ Key: "+ characterData.Key 
                + ",Name:" + characterData.Name 
                + ",Level:" + characterData.Level 
                + ",Hp:" + characterData.Hp 
                + ",Attack:" + characterData.Attack 
                + ",Defence:" + characterData.Defence 
                + ",Critical:" + characterData.Critical 
                + ",Type:" + characterData.Type 
                + ",Position:" + characterData.Position 
                + ",Rarity:" + characterData.Rarity 
                + "}");
        }

        print("==========================================");
    }
}
