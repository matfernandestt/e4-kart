using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSettings", menuName = "Settings/Global Settings")]
public class GlobalSettingsData : ScriptableObject
{
    public int agentSelectionTime = 30;
    public PlayerData localPlayerData;
    
    public int agentBaseMovementSpeed = 6;
    public float mouseSensitivity = 1f;

    public MapData chosenMap;

    public void SetChosenMap(MapData data) { chosenMap = data; }
    public MapData GetChosenMap => chosenMap;
    
    private static GlobalSettingsData _instance;
    public static GlobalSettingsData Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = Resources.Load<GlobalSettingsData>("GlobalSettings");
            return _instance;
        }
    }
}