using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class SaveData
{
    [JsonProperty]
    public int LastClearStage;
    [JsonProperty]
    public Vector3 LastStandingPos;
    [JsonProperty] 
    public bool IsJoinTutorials = false;
}

public class SaveSettingData
{
    [JsonProperty]
    public InputSettings InputSetting;
}

public static class SaveDataManager
{
    public static SaveData SaveData;
    public static SaveSettingData SaveSettingData;
    private static readonly string filePath = Path.Combine(Application.streamingAssetsPath, "SaveData.json");
    private static readonly string keyBindingPath = Path.Combine(Application.streamingAssetsPath, "KeySetting.json");


    public static void Save()
    {
        string json = JsonConvert.SerializeObject(SaveData, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }



    public static void Load()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveData = JsonConvert.DeserializeObject<SaveData>(json);
        }
        else
        {
            SaveData = new SaveData();
            Save();
        }
    }
    
    public static void SaveSettings()
    {
        string json = JsonConvert.SerializeObject(SaveSettingData, Formatting.Indented);
        File.WriteAllText(keyBindingPath, json);

    }
    public static void LoadSettings()
    {
        if (File.Exists(keyBindingPath))
        {
            string json = File.ReadAllText(keyBindingPath);
            SaveSettingData = JsonConvert.DeserializeObject<SaveSettingData>(json);
        }
        else
        {
            SaveSettingData = new SaveSettingData();
            SaveSettings();
        }
    }

    public static void ClearSave()
    {
        File.Delete(filePath);
        Load();
    }
    
    public static void SetInputSetting(InputSettings inputSetting)
    {
        SaveSettingData = new SaveSettingData { InputSetting = inputSetting };
        SaveSettings();
    }
    
    public static void SetLastStandingPos(Vector3 pos)
    {
        SaveData.LastStandingPos = pos;
        Save();
    }
    public static void SetLastClearStage(int stage)
    {
        if (SaveData.LastClearStage < stage)
        {
            SaveData.LastClearStage = stage;
            Save(); 
        }
    }
}