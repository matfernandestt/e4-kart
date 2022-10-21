using System;
using System.Collections;
using Photon;
using UnityEngine;
using ExitGames.Client.Photon;

public class RaceController : PunBehaviour
{
    private RaceAnnouncer startAnnouncer;
    private KartPositioners kartPositioners;
    private PhotonPlayer[] playerList;

    public static Action onStartRace;
    public static Action onFinishRace;
    
    public const string CustomProperty_RaceWinner = "raceWinner";

    private void Awake()
    {
        playerList = PhotonNetwork.playerList;

        onFinishRace += OnFinishRace;
    }

    private void OnDestroy()
    {
        onFinishRace -= OnFinishRace;
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
        if (MatchInstance.CurrentMatch.playerIsLeader)
        {
            var mapName = GlobalSettingsData.Instance.GetChosenMap.gameMapPrefab.name;
            PhotonNetwork.Instantiate(mapName, Vector3.zero, Quaternion.identity, 0);
        }
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

    [PunRPC]
    public void OnFinishRace()
    {
        onFinishRace -= OnFinishRace;
        photonView.RPC("OnFinishRace", PhotonTargets.Others);
        photonView.RPC("ForceOtherPlayersToFinishRace", PhotonTargets.Others);

        var karts = FindObjectsOfType<BaseKart>(true);
        foreach (var kart in karts)
        {
            kart.DisableController();
        }
    }

    [PunRPC]
    public void ForceOtherPlayersToFinishRace()
    {
        onFinishRace?.Invoke();
    }
    
    public static void SetCustomProperty(string customPropertyName, object value)
    {
        var room = PhotonNetwork.room;
        if (room.CustomProperties.ContainsKey(customPropertyName))
        {
            room.CustomProperties.Remove(customPropertyName);
        }
        
        var hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(customPropertyName, value);
        room.SetCustomProperties(hash);
    }
    
    public static object GetCustomProperty(string customPropertyName)
    { 
        return PhotonNetwork.room.CustomProperties[customPropertyName];
    }
}