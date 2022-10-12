using System.Linq;
using UnityEngine;

public class ConnectedPlayersGrid : MonoBehaviour
{
    [SerializeField] private AgentSelectionPlayer[] players;

    private void Awake()
    {
        players = GetComponentsInChildren<AgentSelectionPlayer>();
    }

    public void SetPlayers()
    {
        foreach (var player in players) { player.SetNoPlayer(); }
        
        var connectedPlayers = PhotonNetwork.playerList;
        var playerList = connectedPlayers.ToList();
        for (var i = 0; i < playerList.Count; i++)
        {
            var p = playerList[i];
            if (i < players.Length)
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
