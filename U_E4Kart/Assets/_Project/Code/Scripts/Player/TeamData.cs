using UnityEngine;

[CreateAssetMenu(fileName = "TeamData", menuName = "Player/Team Data")]
public class TeamData : ScriptableObject
{
    public string TeamAName = "Team A";
    public Color TeamAColor;
    public string TeamBName = "Team B";
    public Color TeamBColor;
    
    private static TeamData _instance;
    public static TeamData Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = Resources.Load<TeamData>("TeamData");
            return _instance;
        }
    }
}
