using System;
using System.Collections;
using Photon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentSelection : PunBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button confirmAgentButton;
    [SerializeField] private GameObject agentSelectionBlocker;

    [SerializeField] private TeamGrid teamAGrid;
    [SerializeField] private TeamGrid teamBGrid;

    private Coroutine agentSelectionCoroutine;
    private WaitForSeconds oneSecond = new WaitForSeconds(1f);
    private AgentSelectionPlayer agentSelectionPlayer;

    public static Action<AgentSelectionPlayer> onSpawnLocalAgentSelectionPlayer;
    public static Action<AgentData> onLocalPlayerSelectAgent;

    private void Awake()
    {
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
        teamAGrid.SetPlayers();
        teamBGrid.SetPlayers();
    }

    public void InitialState()
    {
        confirmAgentButton.interactable = true;
        agentSelectionBlocker.SetActive(false);
    }

    public void StartAgentSelection()
    {
        UpdatePlayers();
        
        var currentTime = GlobalSettingsData.Instance.agentSelectionTime;
        
        if(agentSelectionCoroutine != null)
            StopCoroutine(agentSelectionCoroutine);
        agentSelectionCoroutine = StartCoroutine(UpdateTimer());

        IEnumerator UpdateTimer()
        {
            timerText.text = currentTime.ToString();
            while (currentTime > 0)
            {
                yield return oneSecond;
                currentTime--;
                timerText.text = currentTime.ToString();
            }
            MatchInstance.CurrentMatch.onStartMatch?.Invoke();
        }
    }

    private void SetLocalAgentSelectionPlayer(AgentSelectionPlayer localPlayer)
    {
        agentSelectionPlayer = localPlayer;
    }

    private void LocalPlayerSelectedAgent(AgentData agentData)
    {
        agentSelectionPlayer.SetAgent(agentData);
        PlayerData.SetCustomProperty(PhotonNetwork.player, "selectedAgent", AgentDataCollection.GetAgentId(agentData));
    }

    private void OnConfirmAgent()
    {
        confirmAgentButton.interactable = false;
        agentSelectionBlocker.SetActive(true);

        photonView.RPC("UpdatePlayers", PhotonTargets.All);
    }
}
