using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConnectedPlayersGrid : MonoBehaviour
{
    [SerializeField] private Transform playersParent;
    [SerializeField] private AgentSelectionPlayer characterSelectionPlayerPrefab;
    
    private List<AgentSelectionPlayer> players = new List<AgentSelectionPlayer>();
    private bool firstTime;

    public void SetPlayers()
    {
        var connectedPlayers = PhotonNetwork.playerList;
        if (!firstTime)
        {
            var previousList = GetComponentsInChildren<AgentSelectionPlayer>();
            foreach (var player in previousList)
            {
                Destroy(player.gameObject);
            }
            foreach (var player in connectedPlayers)
            {
                var newCharSelectPlayer = Instantiate(characterSelectionPlayerPrefab, playersParent);
                newCharSelectPlayer.SetNoPlayer();
                players.Add(newCharSelectPlayer);
            }

            firstTime = true;
        }

        var playerList = connectedPlayers.ToList();
        for (var i = 0; i < playerList.Count; i++)
        {
            var p = playerList[i];
            if (i < players.Count)
            {
                var agentId = PlayerData.GetCustomProperty(p, "selectedAgent");
                if (agentId != null)
                {
                    var agent = CharacterDataCollection.GetAgentObject((int) agentId);
                    players[i].Setup(p.NickName, agent);
                }

                if (p == PhotonNetwork.player)
                {
                    players[i].SetLocalPlayer();
                    CharacterSelection.onSpawnLocalAgentSelectionPlayer?.Invoke(players[i]);
                }
            }
        }
    }
}
