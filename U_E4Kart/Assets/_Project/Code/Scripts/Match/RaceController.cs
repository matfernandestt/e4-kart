using System;
using System.Collections;
using Photon;
using UnityEngine;

public class RaceController : PunBehaviour
{
    private RaceAnnouncer startAnnouncer;
    private KartPositioners kartPositioners;
    private PhotonPlayer[] playerList;

    public static Action onStartRace;

    private void Awake()
    {
        playerList = PhotonNetwork.playerList;
    }

    private IEnumerator Start()
    {
        if (!PhotonNetwork.connected) yield break;
        var mapsAlreadySpawned = FindObjectsOfType<GameMap>();
        foreach (var map in mapsAlreadySpawned)
        {
            Destroy(map.gameObject);
        }

        yield return new WaitForSeconds(1f);
        var mapName = GlobalSettingsData.Instance.GetChosenMap.gameMapPrefab.name;
        PhotonNetwork.Instantiate(mapName, Vector3.zero, Quaternion.identity, 0);
        yield return new WaitForSeconds(1f);
        startAnnouncer = FindObjectOfType<RaceAnnouncer>(true);
        kartPositioners = FindObjectOfType<KartPositioners>(true);
        
        startAnnouncer.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        startAnnouncer.gameObject.SetActive(true);
        Transitioner.FadeOut();
        
        var position = Vector3.zero;
        position = kartPositioners.GetPositionFromId(PhotonNetwork.player.ID - 1).position;

        var charName = CharacterDataCollection.GetAgentObject((int)PlayerData.GetCustomProperty(PhotonNetwork.player, "selectedAgent")).characterPrefab.name;
        PhotonNetwork.Instantiate("Characters/" + charName, position, Quaternion.identity, 0);
    }
}