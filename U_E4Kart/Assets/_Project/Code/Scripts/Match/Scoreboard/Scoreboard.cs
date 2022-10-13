using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private ScrollRect scoreboardScrollView;
    [SerializeField] private ScoreboardPlayer scoreboardPlayerPrefab;

    private List<ScoreboardPlayer> activePlayers = new List<ScoreboardPlayer>();

    public void InitializeScoreboard(PhotonPlayer[] players = null)
    {
        foreach (Transform child in scoreboardScrollView.content)
        {
            Destroy(child.gameObject);
        }

        if (players != null)
        {
            for (var i = 0; i < players.Length; i++)
            {
                var player = players[i];
                var newPlayer = Instantiate(scoreboardPlayerPrefab, scoreboardScrollView.content);
                newPlayer.Setup(i + 1, player.NickName);
                activePlayers.Add(newPlayer);
            }
        }
    }

    public void UpdateScoreboard(PhotonPlayer[] players)
    {
        for (var i = 0; i < activePlayers.Count; i++)
        {
            var player = activePlayers[i];
            player.UpdatePlace(i + 1);
        }
    }
}
