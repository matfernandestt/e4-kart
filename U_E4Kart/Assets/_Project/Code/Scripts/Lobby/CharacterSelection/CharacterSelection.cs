using System;
using System.Collections;
using Photon;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterSelection : PunBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button confirmAgentButton;
    [SerializeField] private GameObject agentSelectionBlocker;

    [SerializeField] private ConnectedPlayersGrid connectedPlayersGrid;

    private SelectableAgentBase[] selectableAgents;

    private Coroutine agentSelectionCoroutine;
    private WaitForSeconds oneSecond = new WaitForSeconds(1f);
    private AgentSelectionPlayer agentSelectionPlayer;
    private CharacterData localSelectedCharacter;

    public static Action<AgentSelectionPlayer> onSpawnLocalAgentSelectionPlayer;
    public static Action<CharacterData> onLocalPlayerSelectAgent;

    private void Awake()
    {
        selectableAgents = GetComponentsInChildren<SelectableAgentBase>(true);
        
        onSpawnLocalAgentSelectionPlayer += SetLocalAgentSelectionPlayer;
        onLocalPlayerSelectAgent += LocalPlayerSelectedAgent;
        
        confirmAgentButton.onClick.AddListener(OnConfirmAgent);
    }

    private void OnDestroy()
    {
        onSpawnLocalAgentSelectionPlayer -= SetLocalAgentSelectionPlayer;
        onLocalPlayerSelectAgent -= LocalPlayerSelectedAgent;
    }

    [PunRPC]
    public void UpdatePlayers()
    {
        connectedPlayersGrid.SetPlayers();
    }

    public void InitialState()
    {
        confirmAgentButton.interactable = true;
        agentSelectionBlocker.SetActive(false);
    }

    public void StartCharacterSelection()
    {
        UpdatePlayers();
    }

    private void SetLocalAgentSelectionPlayer(AgentSelectionPlayer localPlayer)
    {
        agentSelectionPlayer = localPlayer;
    }

    private void LocalPlayerSelectedAgent(CharacterData characterData)
    {
        agentSelectionPlayer.SetAgent(characterData);
        localSelectedCharacter = characterData;
    }

    private void OnConfirmAgent()
    {
        confirmAgentButton.interactable = false;
        agentSelectionBlocker.SetActive(true);
        PlayerData.SetCustomProperty(PhotonNetwork.player, "selectedAgent", CharacterDataCollection.GetAgentId(localSelectedCharacter));

        photonView.RPC("UpdatePlayers", PhotonTargets.All);
        
        
        //THIS WILL START THE MATCH
        MatchInstance.CurrentMatch.onStartMatch?.Invoke();
    }

    public void DisableAll(GameObject current)
    {
        foreach (var selectableAgent in selectableAgents)
        {
            if(selectableAgent.gameObject != current)
                selectableAgent.DisableToggle();
        }
    }
}
