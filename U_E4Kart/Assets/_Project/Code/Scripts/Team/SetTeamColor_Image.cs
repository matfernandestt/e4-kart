using System;
using UnityEngine;
using UnityEngine.UI;

public class SetTeamColor_Image : MonoBehaviour
{
    [SerializeField] private PunTeams.Team team;
    
    private void Awake()
    {
        var image = GetComponent<Image>();
        if (image != null)
        {
            switch (team)
            {
                case PunTeams.Team.blue:
                    image.color = TeamData.Instance.TeamAColor;
                    break;
                case PunTeams.Team.red:
                    image.color = TeamData.Instance.TeamBColor;
                    break;
            }
        }
    }
}