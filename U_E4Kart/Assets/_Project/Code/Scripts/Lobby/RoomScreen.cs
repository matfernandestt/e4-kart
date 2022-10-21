using System;
using System.Collections;
using Photon;
using TMPro;
using UnityEngine;
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

    [SerializeField] private Image selectedMap;
    [SerializeField] private TextMeshProUGUI selectedMapName;
    
    [SerializeField] private CharacterSelection selectCharacterScreen;
    [SerializeField] private MapSelectionScreen mapSelectionScreen;

    public Action onLeaveRoom;

    private void Awake()
    {
        selectCharacterScreen.gameObject.SetActive(false);
        
        selectMapButton.onClick.AddListener(SelectMap);
        startMatchButton.onClick.AddListener(StartMatch);
        quitRoomButton.onClick.AddListener(QuitRoom);

        GlobalSettingsData.Instance.onMapSelected += RefreshSelectedMap;
    }

    private void OnDestroy()
    {
        GlobalSettingsData.Instance.onMapSelected -= RefreshSelectedMap;
    }

    public override void OnJoinedRoom()
    {
        SetupRoomScreen();
        startMatchButton.interactable = false;
        
        RefreshSelectedMap();
    }

    public void EnableSelectMapButton(bool enable)
    {
        selectMapButton.gameObject.SetActive(enable);
        mapSelectionScreen.RefreshMapSelectionScreen();
    }

    public void UpdateMapSelection()
    {
        RefreshSelectedMap();
    }

    [PunRPC]
    public void RefreshSelectedMap()
    {
        var chosenMap = GlobalSettingsData.Instance.GetChosenMap;
        if (MatchInstance.CurrentMatch.playerIsLeader)
        {
            startMatchButton.interactable = GlobalSettingsData.Instance.GetChosenMap != null;
            photonView.RPC("RefreshSelectedMap", PhotonTargets.Others);
            if (GlobalSettingsData.Instance.chosenMap != null)
            {
                selectedMap.sprite = chosenMap.mapIcon;
                selectedMapName.text = chosenMap.mapName;
            }
        }
        else
        {
            var leader = PlayerData.GetLeader();
            if (leader != null)
            {
                var selectedMapProperty = leader.CustomProperties[PlayerData.CustomProperty_SelectedMap];
                if (selectedMapProperty != null)
                {
                    var currentLeaderMap = MapDataCollection.GetMapObject((int) selectedMapProperty);
                    if (currentLeaderMap != null)
                    {
                        GlobalSettingsData.Instance.SetLowkeyChosenMap(currentLeaderMap);
                        selectedMap.sprite = currentLeaderMap.mapIcon;
                        selectedMapName.text = currentLeaderMap.mapName;
                    }
                }
            }
        }
    }
    
    [PunRPC]
    public void SetupRoomScreen()
    {
        if (!MatchInstance.CurrentMatch.playerIsLeader)
        {
            startMatchButton.interactable = false;
            RefreshSelectedMap();
        }

        startMatchButtonText.text = MatchInstance.CurrentMatch.playerIsLeader ? StartGame : WaitingGame;

        foreach (Transform child in teamAScrollView.content.transform)
            Destroy(child.gameObject);

        var players = PhotonNetwork.playerList;
        foreach (var player in players)
        {
            var newPlayer = Instantiate(roomPlayerPrefab, teamAScrollView.content);
            var isLeader = (bool) PlayerData.GetCustomProperty(player, PlayerData.CustomProperty_IsRoomLeader);
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
            startMatchButton.interactable = false;
            selectMapButton.interactable = false;
            quitRoomButton.interactable = false;
            Transitioner.Begin();
            while (Transitioner.IsTransitioning) yield return null;
            selectCharacterScreen.gameObject.SetActive(true);
            selectCharacterScreen.InitialState();
            selectCharacterScreen.StartCharacterSelection();
        }
    }

    [PunRPC]
    public void QuitRoom()
    {
        if (MatchInstance.CurrentMatch.playerIsLeader)
        {
            photonView.RPC("QuitRoom", PhotonTargets.Others);
            MatchInstance.CurrentMatch.playerIsLeader = false;
            PlayerData.SetCustomProperty(PhotonNetwork.player, PlayerData.CustomProperty_IsRoomLeader, false);
        }
        PhotonNetwork.LeaveRoom();
        photonView.RPC("SetupRoomScreen", PhotonTargets.All);
        onLeaveRoom?.Invoke();
    }
}
