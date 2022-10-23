using System;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    private void Awake()
    {
        SaveSystem.Load();
        GlobalSettingsData.Instance.Set_BGM_Volume(GlobalSettingsData.Instance.loadedSave.save.Volume_BGM);
        GlobalSettingsData.Instance.Set_SFX_Volume(GlobalSettingsData.Instance.loadedSave.save.Volume_SFX);
    }
}
