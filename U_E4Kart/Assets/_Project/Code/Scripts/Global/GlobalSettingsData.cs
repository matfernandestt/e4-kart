using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSettings", menuName = "Settings/Global Settings")]
public class GlobalSettingsData : ScriptableObject
{
    public int agentSelectionTime = 30;
    public PlayerData localPlayerData;
    
    public float mouseSensitivity = 1f;
    public Vector3 cameraPositionOffset;
    public Vector3 cameraRotationOffset;
    public float cameraFollowDelay;

    public void SetChosenMap(MapData data)
    {
        chosenMap = data;
        PlayerData.SetCustomProperty(PhotonNetwork.player, PlayerData.CustomProperty_SelectedMap, MapDataCollection.GetMapId(data));
        onMapSelected?.Invoke();
    }

    public void SetLowkeyChosenMap(MapData data)
    {
        chosenMap = data;
    }
    public MapData GetChosenMap => chosenMap;
    
    public MapData chosenMap;

    public Action onMapSelected;
    
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