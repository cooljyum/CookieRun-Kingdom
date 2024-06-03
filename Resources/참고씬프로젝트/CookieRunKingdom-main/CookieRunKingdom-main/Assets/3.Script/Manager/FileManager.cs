using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileManager
{
    private SaveData _saveData = new SaveData();
    public SaveData SaveData => _saveData;

    public void Init()
    {
        LoadGame();
    }

    public void SetSaveData(SaveData saveData)
    {
        _saveData = saveData;
    }

    // 저장하기
    public void SaveGame()
    {
        string filePath = Application.persistentDataPath + "/ckData.json";

        StreamWriter saveFile = new StreamWriter(filePath);
        saveFile.Write(JsonUtility.ToJson(_saveData, true));

        saveFile.Close();
    }

    // 불러오기
    public void LoadGame()
    {
        string filePath = Application.persistentDataPath + "/ckData.json";
        Debug.Log(filePath);

        if(!File.Exists(filePath))
        {
            Debug.Log("No file!");
            SaveGame();
            return;
        }

        StreamReader saveFile = new StreamReader(filePath);
        JsonUtility.FromJsonOverwrite(saveFile.ReadToEnd(), _saveData);

        saveFile.Close();
    }
}
