using System;
using System.Collections;
using Photon;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterSelection : PunBehaviour
{
    [SerializeField] private Button confirmAgentButton;
    [SerializeField] private TextMeshProUGUI confirmCharacterButtonText;
    [SerializeField] private GameObject agentSelectionBlocker;

    [SerializeField] private ConnectedPlayersGrid connectedPlayersGrid;
    [SerializeField] private Timer startMatchTimer;
    [SerializeField] private CharacterShowcase characterShowcase;

    [SerializeField] private AudioClip secretUnlock;

    private SelectableAgentBase[] selectableAgents;

    private Coroutine agentSelectionCoroutine;
    private AgentSelectionPlayer agentSelectionPlayer;
    private CharacterData localSelectedCharacter;

    public static Action<AgentSelectionPlayer> onSpawnLocalAgentSelectionPlayer;
    public static Action<CharacterData> onLocalPlayerSelectAgent;
    public static Action<CharacterData> onSelectSecretCharacter;

    private void Awake()
    {
        selectableAgents = GetComponentsInChildren<SelectableAgentBase>(true);
        
        onSpawnLocalAgentSelectionPlayer += SetLocalAgentSelectionPlayer;
        onLocalPlayerSelectAgent += LocalPlayerSelectedAgent;

        confirmAgentButton.onClick.AddListener(OnConfirmAgent);
        
        startMatchTimer.gameObject.SetActive(false);
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
        
        CheckAllPlayersReady();
    }

    public void InitialState()
    {
        confirmCharacterButtonText.text = "Select a character";
        confirmAgentButton.interactable = false;
        agentSelectionBlocker.SetActive(false);
        
        onSelectSecretCharacter += SelectSecretCharacter;
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
        confirmCharacterButtonText.text = "Confirm character";
        confirmAgentButton.interactable = true;
        
        agentSelectionPlayer.SetAgent(characterData);
        localSelectedCharacter = characterData;
        
        characterShowcase.UpdateCharacter(characterData.characterPrefab.subKart.GetVisuals);
    }

    private void SelectSecretCharacter(CharacterData characterData)
    {
        var src = Pool_SFX.Instance.GetInstance(Vector3.zero);
        src.clip = secretUnlock;
        src.Play();
        Pool_SFX.Instance.ReturnInstanceWhenConcludePlaying(src);
        
        onSelectSecretCharacter = null;
        agentSelectionPlayer.SetAgent(characterData);
        localSelectedCharacter = characterData;
        characterShowcase.UpdateCharacter(characterData.characterPrefab.subKart.GetVisuals);
        
        OnConfirmAgent();
    }

    private void OnConfirmAgent()
    {
        var src = Pool_SFX.Instance.GetInstance(Vector3.zero);
        src.clip = localSelectedCharacter.sfx_OnSelectCharacter;
        src.Play();
        Pool_SFX.Instance.ReturnInstanceWhenConcludePlaying(src);
        
        confirmCharacterButtonText.text = "Waiting for match to start";
        characterShowcase.EndShowcase();
        confirmAgentButton.interactable = false;
        agentSelectionBlocker.SetActive(true);
        PlayerData.SetCustomProperty(PhotonNetwork.player, PlayerData.CustomProperty_SelectedCharacter, CharacterDataCollection.GetAgentId(localSelectedCharacter));

        photonView.RPC("UpdatePlayers", PhotonTargets.All);
    }

    private void CheckAllPlayersReady()
    {
        var playersReady = 0;
        
        var players = PhotonNetwork.playerList;
        foreach (var player in players)
        {
            var selectedAgent = (int) player.CustomProperties[PlayerData.CustomProperty_SelectedCharacter];
            if (selectedAgent != null)
            {
                if (selectedAgent != 0)
                    playersReady++;
            }
        }
        
        if (playersReady == players.Length)
        {
            startMatchTimer.gameObject.SetActive(true);
            startMatchTimer.Countdown(10, StartMatch);
        }
    }

    private void StartMatch()
    {
        onSelectSecretCharacter = null;
        Transitioner.FadeIn();

        StartCoroutine(WaitToStart());
        IEnumerator WaitToStart()
        {
            yield return new WaitForSeconds(1f);
            MatchInstance.CurrentMatch.onStartMatch?.Invoke();
        }
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
