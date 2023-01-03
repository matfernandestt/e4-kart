using UnityEditor;

public class SaveSystemEditor
{
    [MenuItem("Save System/Save")]
    private static void Save()
    {
        SaveSystem.Save();
    }
    
    [MenuItem("Save System/Load")]
    private static void Load()
    {
        SaveSystem.Load();
        EditorUtility.SetDirty(GlobalSettingsData.Instance.loadedSave);
    }
    
    [MenuItem("Save System/Delete Save File")]
    private static void DeleteSaveFile()
    {
        SaveSystem.DeleteSaveFile();
    }
    
    [MenuItem("Save System/Open Save File")]
    private static void OpenSaveFile()
    {
        SaveSystem.Open();
    }
}
