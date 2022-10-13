using System;
using System.Collections;
using Photon;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RoomScreen : PunBehaviour
{
    [SerializeField] private string StartGame = "START GAME";
    [SerializeField] private string WaitingGame = "WAIT ROOM LEADER START";
    [SerializeField] private PlayerData playerData;
    
    [SerializeField] private ScrollRect teamAScrollView;
    [SerializeField] private RoomPlayer roomPlayerPrefab;

    [SerializeField] private Button selectMapButton;
    [SerializeField] private Button startMatchButton;
    [SerializeField] private Button quitRoomButton;
    [SerializeField] private TextMeshProUGUI startMatchButtonText;
    
    [FormerlySerializedAs("selectAgentScreen")] [SerializeField] private CharacterSelection selectCharacterScreen;
    [SerializeField] private MapSelectionScreen mapSelectionScreen;

    public Action onLeaveRoom;

    private void Awake()
    {
        selectCharacterScreen.gameObject.SetActive(false);
        
        selectMapButton.onClick.AddListener(SelectMap);
        startMatchButton.onClick.AddListener(StartMatch);
        quitRoomButton.onClick.AddListener(QuitRoom);
    }

    public override void OnJoinedRoom()
    {
        SetupRoomScreen();
    }

    public void EnableSelectMapButton(bool enable)
    {
        selectMapButton.gameObject.SetActive(enable);
        mapSelectionScreen.RefreshMapSelectionScreen();
    }
    
    [PunRPC]
    public void SetupRoomScreen()
    {
        startMatchButtonText.text = MatchInstance.CurrentMatch.playerIsLeader ? StartGame : WaitingGame;
        startMatchButton.interactable = MatchInstance.CurrentMatch.playerIsLeader;
        
        foreach (Transform child in teamAScrollView.content.transform)
            Destroy(child.gameObject);

        var players = PhotonNetwork.playerList;
        foreach (var player in players)
        {
            var newPlayer = Instantiate(roomPlayerPrefab, teamAScrollView.content);
            var isLeader = (bool) PlayerData.GetCustomProperty(player, "isRoomLeader");
            newPlayer.Setup(player.NickName, player.GetTeam(), isLeader);
        }
    }

    private void SelectMap()
    {
        mapSelectionScreen.gameObject.SetActive(true);
    }

    [PunRPC]
    private void StartMatch()
    {
        if (MatchInstance.CurrentMatch.playerIsLeader)
        {
            photonView.RPC("StartMatch", PhotonTargets.Others);
        }
        
        StartCoroutine(Transition());
        IEnumerator Transition()
        {
            Transitioner.Begin();
            while (Transitioner.IsTransitioning) yield return null;
            selectCharacterScreen.gameObject.SetActive(true);
            selectCharacterScreen.InitialState();
            selectCharacterScreen.StartCharacterSelection();
        }
    }

    private void QuitRoom()
    {
        PhotonNetwork.LeaveRoom();
        photonView.RPC("SetupRoomScreen", PhotonTargets.All);
        onLeaveRoom?.Invoke();
    }
}
