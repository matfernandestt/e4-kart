using System;
using Photon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomScreen : PunBehaviour
{
    [SerializeField] private PlayerData playerData;
    
    [SerializeField] private ScrollRect teamAScrollView;
    [SerializeField] private ScrollRect teamBScrollView;
    [SerializeField] private Button selectTeamAButton;
    [SerializeField] private Button selectTeamBButton;
    [SerializeField] private RoomPlayer roomPlayerPrefab;

    [SerializeField] private Button selectMapButton;
    [SerializeField] private Button startMatchButton;
    [SerializeField] private Button quitRoomButton;
    [SerializeField] private TextMeshProUGUI startMatchButtonText;
    
    [SerializeField] private AgentSelection selectAgentScreen;
    [SerializeField] private MapSelectionScreen mapSelectionScreen;

    public Action onLeaveRoom;

    private void Awake()
    {
        startMatchButton.interactable = false;
        selectAgentScreen.gameObject.SetActive(false);
        selectTeamAButton.onClick.AddListener(() => ChangeTeam(PunTeams.Team.blue));
        selectTeamBButton.onClick.AddListener(() => ChangeTeam(PunTeams.Team.red));
        
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
        startMatchButtonText.text = MatchInstance.CurrentMatch.playerIsLeader ? "START GAME" : "WAITING START";
        
        foreach (Transform child in teamAScrollView.content.transform)
            Destroy(child.gameObject);
        foreach (Transform child in teamBScrollView.content.transform)
            Destroy(child.gameObject);

        MatchInstance.ResetPlayers();
        var players = PhotonNetwork.playerList;
        foreach (var player in players)
        {
            var newPlayer = Instantiate(roomPlayerPrefab, player.GetTeam() == PunTeams.Team.blue ? teamAScrollView.content : teamBScrollView.content);
            var isLeader = (bool) PlayerData.GetCustomProperty(player, "isRoomLeader");
            newPlayer.Setup(player.NickName, player.GetTeam(), isLeader);
            MatchInstance.AddPlayerToTeam(player.GetTeam());
            
        }

        startMatchButton.interactable = MatchInstance.DoesTheMatchHaveEnoughPlayersToStart() && MatchInstance.CurrentMatch.playerIsLeader;
    }

    private void ChangeTeam(PunTeams.Team team)
    {
        playerData.team = team;
        PhotonNetwork.player.SetTeam(team);
        
        photonView.RPC("SetupRoomScreen", PhotonTargets.All);
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

        selectAgentScreen.gameObject.SetActive(true);
        selectAgentScreen.InitialState();
        selectAgentScreen.StartAgentSelection();
    }

    private void QuitRoom()
    {
        PhotonNetwork.LeaveRoom();
        photonView.RPC("SetupRoomScreen", PhotonTargets.All);
        onLeaveRoom?.Invoke();
    }
}
