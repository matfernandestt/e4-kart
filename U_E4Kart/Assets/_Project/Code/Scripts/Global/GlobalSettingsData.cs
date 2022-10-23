using System;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "GlobalSettings", menuName = "Settings/Global Settings")]
public class GlobalSettingsData : ScriptableObject
{
    public SaveData loadedSave;
    
    public byte maxPlayersPerRoom = 5;
    public int agentSelectionTime = 30;
    public PlayerData localPlayerData;
    
    public Vector3 cameraPositionOffset;
    public Vector3 cameraRotationOffset;
    public float cameraFollowDelay;

    public AudioMixer bgmMixer;
    public AudioMixer sfxMixer;

    public void Set_BGM_Volume(float volume)
    {
        bgmMixer.SetFloat("Volume", volume);
        loadedSave.save.Volume_BGM = volume;
    }

    public float Get_BGM_Volume()
    {
        var volume = 0f;
        bgmMixer.GetFloat("Volume", out volume);
        return volume;
    }
    
    public void Set_SFX_Volume(float volume)
    {
        sfxMixer.SetFloat("Volume", volume);
        loadedSave.save.Volume_SFX = volume;
    }
    
    public float Get_SFX_Volume()
    {
        var volume = 0f;
        sfxMixer.GetFloat("Volume", out volume);
        return volume;
    }

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
    
    [HideInInspector] public MapData chosenMap;

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