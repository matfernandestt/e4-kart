using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class SaveSystem
{
    private const string fileName = "SaveFile";
    private const string extension = "e4";
    
    public static void DeleteSaveFile()
    {
        var path = $"{Application.persistentDataPath}/{fileName}.{extension}";

        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found!");
            return;
        }
        
        File.Delete(path);
    }
    
    public static void Load()
    {
        var path = $"{Application.persistentDataPath}/{fileName}.{extension}";

        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found!");
            return;
        }
        GlobalSettingsData.Instance.loadedSave.save = JsonUtility.FromJson<Save>(File.ReadAllText(path));
    }

    public static void Save()
    {
        var path = $"{Application.persistentDataPath}/{fileName}.{extension}";
        File.WriteAllText(path, JsonUtility.ToJson(GlobalSettingsData.Instance.loadedSave.save));
    }
    
    public static void Open()
    {
        var path = $"{Application.persistentDataPath}/{fileName}.{extension}";
        System.Diagnostics.Process.Start("C:/Windows/notepad.exe", path);
    }
}

[Serializable]
public class Save
{
    public string nickname;
    public float Volume_BGM = -20f;
    public float Volume_SFX = -20f;
    public bool showPing = true;
    public bool showFPS = true;
}