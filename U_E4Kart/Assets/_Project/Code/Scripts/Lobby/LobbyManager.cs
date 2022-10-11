using System;
using System.Collections;
using ExitGames.Client.Photon;
using Photon;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum LobbyScreen
{
    Connect,
    Lobby,
    Room,
}

public class LobbyManager : PunBehaviour
{
    [SerializeField] private PlayerData playerData;
    [Space]
    [SerializeField] private TextMeshProUGUI connectionStatus;
    [SerializeField] private GameObject loadingScreen;
    
    [SerializeField] private GameObject connectScreen;
    [SerializeField] private GameObject lobbyScreen;
    [SerializeField] private RoomScreen roomScreen;
    
    [Header("Connect Screen")]
    [SerializeField] private TMP_InputField nicknameInputField;
    [SerializeField] private Button connectButton;
    [SerializeField] private GameObject mustTypeNickname;

    [Header("Lobby Screen")]
    [SerializeField] private ScrollRect roomScrollView;
    [SerializeField] private Button refreshRoomListButton;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private RoomButton roomButtonPrefab;

    [Header("Room")]
    [SerializeField] private byte maxPlayersInRooms;

    private Coroutine lobbyScreenCoroutine;
    private LobbyScreen currentLobbyScreen;

    private void Awake()
    {
        SetActiveScreen(LobbyScreen.Connect);
        if (!string.IsNullOrEmpty(playerData.nickname))
        {
            nicknameInputField.text = playerData.nickname;
        }

        mustTypeNickname.SetActive(false);
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = true;
        EnableLoading(false);
        
        connectButton.onClick.AddListener(Connect);
        refreshRoomListButton.onClick.AddListener(RefreshRoomList);
        createRoomButton.onClick.AddListener(CreateRoom);

        roomScreen.onLeaveRoom += () => SetActiveScreen(LobbyScreen.Lobby);
    }

    private void OnDestroy()
    {
        roomScreen.onLeaveRoom = null;
    }

    private void Update()
    {
        connectionStatus.text = PhotonNetwork.connectionState.ToString();
    }

    private void SetActiveScreen(LobbyScreen screen)
    {
        connectScreen.SetActive(screen == LobbyScreen.Connect);
        lobbyScreen.SetActive(screen == LobbyScreen.Lobby);
        roomScreen.gameObject.SetActive(screen == LobbyScreen.Room);
        
        if(lobbyScreenCoroutine != null)
            StopCoroutine(lobbyScreenCoroutine);
        
        switch (screen)
        {
            case LobbyScreen.Connect:
                break;
            case LobbyScreen.Lobby:
                OnEnterLobbyScreen();
                RefreshRoomList();
                break;
            case LobbyScreen.Room:
                break;
            default:
                break;
        }
        currentLobbyScreen = screen;
    }

    private void EnableLoading(bool enable)
    {
        loadingScreen.SetActive(enable);
    }

    private void Connect()
    {
        mustTypeNickname.SetActive(string.IsNullOrEmpty(nicknameInputField.text));
        if (string.IsNullOrEmpty(nicknameInputField.text))
        {
            return;
        }
        EnableLoading(true);
        connectButton.interactable = false;

        PhotonNetwork.playerName = nicknameInputField.text;
        playerData.nickname = nicknameInputField.text;

        PhotonNetwork.ConnectUsingSettings("v1.0");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        SetActiveScreen(LobbyScreen.Lobby);
        connectButton.interactable = true;

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        EnableLoading(false);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        
        SetActiveScreen(LobbyScreen.Room);
        if (!MatchInstance.CurrentMatch.playerIsLeader)
        {
            MatchInstance.CurrentMatch.playerIsLeader = false;
            roomScreen.photonView.RPC("SetupRoomScreen", PhotonTargets.All);
        }
        PlayerData.SetCustomProperty(PhotonNetwork.player, "selectedAgent", 0);

        EnableLoading(false);
    }

    private void CreateRoom()
    {
        EnableLoading(true);
        
        var options = new RoomOptions
        {
            MaxPlayers = maxPlayersInRooms
        };
        PhotonNetwork.CreateRoom($"{PhotonNetwork.player.NickName}'s room", options, null);
        MatchInstance.NewMatch();
        MatchInstance.CurrentMatch.playerIsLeader = true;
        MatchInstance.CurrentMatch.onStartMatch += StartGame;

        PlayerData.SetCustomProperty(PhotonNetwork.player, "isRoomLeader", MatchInstance.CurrentMatch.playerIsLeader);

        UpdatePlayerTeam();
        roomScreen.SetupRoomScreen();
        roomScreen.EnableSelectMapButton(true);
    }

    private void RefreshRoomList()
    {
        if(lobbyScreenCoroutine != null)
            StopCoroutine(lobbyScreenCoroutine);
        lobbyScreenCoroutine = StartCoroutine(UpdateLobby());
    }

    private void OnEnterLobbyScreen()
    {
        lobbyScreenCoroutine = StartCoroutine(UpdateLobby());
    }

    private IEnumerator UpdateLobby()
    {
        while (true)
        {
            foreach (Transform roomButtons in roomScrollView.content.transform)
            {
                Destroy(roomButtons.gameObject);
            }
            
            var roomList = PhotonNetwork.GetRoomList();
            foreach (var room in roomList)
            {
                var newRoom = Instantiate(roomButtonPrefab, roomScrollView.content);
                newRoom.SetupButton(room.Name, room.PlayerCount, maxPlayersInRooms, 
                    () =>
                    {
                        MatchInstance.NewMatch();
                        UpdatePlayerTeam();
                        PhotonNetwork.JoinRoom(room.Name);
                        roomScreen.EnableSelectMapButton(false);
                    });
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void UpdatePlayerTeam()
    {
        if (PhotonNetwork.player.GetTeam() == PunTeams.Team.none)
        {
            var match = MatchInstance.CurrentMatch;
            PhotonNetwork.player.SetTeam(match.playersInTeamB < match.playersInTeamA
                ? PunTeams.Team.red
                : PunTeams.Team.blue);
        }
        
        PlayerData.SetCustomProperty(PhotonNetwork.player, "isRoomLeader", MatchInstance.CurrentMatch.playerIsLeader);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("SCN_DefaultScene");
    }
}
