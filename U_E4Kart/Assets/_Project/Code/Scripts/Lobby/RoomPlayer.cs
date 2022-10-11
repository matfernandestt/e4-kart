using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private GameObject leaderCrown;
    [SerializeField] private Image selfImage;
        
    private bool roomLeader;
        
    public void Setup(string name, PunTeams.Team team, bool isRoomLeader)
    {
        playerName.text = name;
        roomLeader = isRoomLeader;
        leaderCrown.SetActive(isRoomLeader);
        switch (team)
        {
            case PunTeams.Team.blue:
                selfImage.color = TeamData.Instance.TeamAColor;
                break;
            case PunTeams.Team.red:
                selfImage.color = TeamData.Instance.TeamBColor;
                break;
        }
    }
}
