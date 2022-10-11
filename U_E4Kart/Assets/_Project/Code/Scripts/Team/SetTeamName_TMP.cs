using TMPro;
using UnityEngine;

public class SetTeamName_TMP : MonoBehaviour
{
    [SerializeField] private PunTeams.Team team;
    
    private void Awake()
    {
        var text = GetComponent<TextMeshProUGUI>();
        if (text != null)
        {
            switch (team)
            {
                case PunTeams.Team.blue:
                    text.text = TeamData.Instance.TeamAName;
                    break;
                case PunTeams.Team.red:
                    text.text = TeamData.Instance.TeamBName;
                    break;
            }
        }
    }
}
