using System;
using PathCreation.Examples;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    [SerializeField] private Material skybox;
    [SerializeField] private RoadMeshCreator roadMeshCreator;
    [SerializeField] private Road road;

    private void Awake()
    {
        RenderSettings.skybox = skybox;
        roadMeshCreator.TriggerUpdate();
        road.RefreshCollision();

        if(GlobalSettingsData.Instance.chosenMap != null)
            BgmManager.PlayBGM(GlobalSettingsData.Instance.chosenMap.bgm);
    }
}
