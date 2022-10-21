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
    [SerializeField] private TextMeshProUGUI welcomeBackTitle;
    [SerializeField] private Button refreshRoomListButton;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button disconnectButton;
    [SerializeField] private RoomButton roomButtonPrefab;
    [SerializeField] private GameObject roomSelectionBlocker;

    [Header("Room")]
    [SerializeField] private byte maxPlayersInRooms;

    [Header("Audio")]
    [SerializeField] private AudioClip menusBGM;
    
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
        disconnectButton.onClick.AddListener(Disconnect);

        roomScreen.onLeaveRoom += LeaveRoom;
    }

    private void OnDestroy()
    {
        roomScreen.onLeaveRoom = null;
    }
    
    private void Start()
    {
        BgmManager.PlayBGM(menusBGM);

        if (PhotonNetwork.connected)
        {
            welcomeBackTitle.text = $"Welcome back, {playerData.nickname}!";
            SetActiveScreen(LobbyScreen.Lobby);
        }
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

    private void Disconnect()
    {
        StartCoroutine(Transition());

        IEnumerator Transition()
        {
            PhotonNetwork.Disconnect();
            Transitioner.Begin();
            while (Transitioner.IsTransitioning) yield return null;
            SetActiveScreen(LobbyScreen.Connect);
        }
    }

    private void EnableLoading(bool enable)
    {
        loadingScreen.SetActive(enable);
    }
    
    private void LeaveRoom()
    {
        StartCoroutine(Transition());

        IEnumerator Transition()
        {
            Transitioner.Begin();
            while (Transitioner.IsTransitioning) yield return null;
            SetActiveScreen(LobbyScreen.Lobby);
        }
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
        welcomeBackTitle.text = $"Welcome back, {playerData.nickname}!";

        PhotonNetwork.ConnectUsingSettings("v1.0");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        StartCoroutine(Transition());
        IEnumerator Transition()
        {
            Transitioner.Begin();
            while (Transitioner.IsTransitioning) yield return null;
            SetActiveScreen(LobbyScreen.Lobby);
            connectButton.interactable = true;

            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        EnableLoading(false);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        StartCoroutine(Transition());
        IEnumerator Transition()
        {
            Transitioner.Begin();
            while (Transitioner.IsTransitioning) yield return null;
            SetActiveScreen(LobbyScreen.Room);
            if (!MatchInstance.CurrentMatch.playerIsLeader)
            {
                MatchInstance.CurrentMatch.playerIsLeader = false;
                roomScreen.photonView.RPC("SetupRoomScreen", PhotonTargets.All);
            }
            PlayerData.SetCustomProperty(PhotonNetwork.player, PlayerData.CustomProperty_SelectedCharacter, 0);

            EnableLoading(false);
        }
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

        PlayerData.SetCustomProperty(PhotonNetwork.player, PlayerData.CustomProperty_IsRoomLeader, MatchInstance.CurrentMatch.playerIsLeader);

        UpdatePlayerTeam();
        roomScreen.SetupRoomScreen();
        roomScreen.EnableSelectMapButton(true);
        GlobalSettingsData.Instance.SetChosenMap(null);
        roomScreen.UpdateMapSelection();
    }

    private void RefreshRoomList()
    {
        if(lobbyScreenCoroutine != null)
            StopCoroutine(lobbyScreenCoroutine);
        lobbyScreenCoroutine = StartCoroutine(UpdateLobby());
    }

    private void OnEnterLobbyScreen()
    {
        roomSelectionBlocker.SetActive(false);
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
                        roomSelectionBlocker.SetActive(true);
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
        }
        
        PlayerData.SetCustomProperty(PhotonNetwork.player, PlayerData.CustomProperty_IsRoomLeader, MatchInstance.CurrentMatch.playerIsLeader);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("SCN_DefaultScene");
    }
}
