using System.Linq;
using UnityEngine;

public class TeamGrid : MonoBehaviour
{
    [SerializeField] private PunTeams.Team team;
    [SerializeField] private AgentSelectionPlayer[] players;

    private void Awake()
    {
        players = GetComponentsInChildren<AgentSelectionPlayer>();
    }

    public void SetPlayers()
    {
        foreach (var player in players) { player.SetNoPlayer(); }
        
        var connectedPlayers = PhotonNetwork.playerList;
        var thisTeamPlayers = connectedPlayers.Where(player => player.GetTeam() == team).ToList();
        for (var i = 0; i < thisTeamPlayers.Count; i++)
        {
            var p = thisTeamPlayers[i];
            if (i < players.Length)
            {
                var agentId = PlayerData.GetCustomProperty(p, "selectedAgent");
                if (agentId != null)
                {
                    var agent = AgentDataCollection.GetAgentObject((int) agentId);
                    players[i].Setup(p.NickName, agent);
                }

                if(p == PhotonNetwork.player)
                    AgentSelection.onSpawnLocalAgentSelectionPlayer?.Invoke(players[i]);
            }
        }
    }
}
