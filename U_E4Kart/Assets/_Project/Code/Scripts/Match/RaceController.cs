using System;
using System.Collections;
using Photon;
using UnityEngine;

public class RaceController : PunBehaviour
{
    [SerializeField] private GameObject startAnnouncer;
    [SerializeField] private KartPositioners kartPositioners;

    private PhotonPlayer[] playerList;

    public static Action onStartRace;

    private void Awake()
    {
        playerList = PhotonNetwork.playerList;
    }

    private IEnumerator Start()
    {
        if (!PhotonNetwork.connected) yield break;
        startAnnouncer.SetActive(false);
        yield return new WaitForSeconds(1f);
        startAnnouncer.SetActive(true);
        Transitioner.FadeOut();

        var position = Vector3.zero;
        position = kartPositioners.GetPositionFromId(PhotonNetwork.player.ID - 1).position;

        var charName = CharacterDataCollection.GetAgentObject((int)PlayerData.GetCustomProperty(PhotonNetwork.player, "selectedAgent")).characterPrefab.name;
        PhotonNetwork.Instantiate("Characters/" + charName, position, Quaternion.identity, 0);
    }
}