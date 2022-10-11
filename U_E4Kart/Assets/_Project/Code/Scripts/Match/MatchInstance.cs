using System;
using UnityEngine;

public class MatchInstance : MonoBehaviour
{
    public static Match CurrentMatch;

    public static void NewMatch() { CurrentMatch = new Match(); }
    
    public static bool DoesTheMatchHaveEnoughPlayersToStart() => CurrentMatch.playersInTeamA > 0 && CurrentMatch.playersInTeamB > 0;

    public static void ResetPlayers()
    {
        CurrentMatch.playersInTeamA = 0;
        CurrentMatch.playersInTeamB = 0;
    }

    public static void AddPlayerToTeam(PunTeams.Team team)
    {
        switch (team)
        {
            case PunTeams.Team.blue:
                CurrentMatch.playersInTeamA++;
                break;
            case PunTeams.Team.red:
                CurrentMatch.playersInTeamB++;
                break;
        }
    }

    public static void RemovePlayerFromTeam(PunTeams.Team team)
    {
        switch (team)
        {
            case PunTeams.Team.blue:
                CurrentMatch.playersInTeamA--;
                break;
            case PunTeams.Team.red:
                CurrentMatch.playersInTeamB--;
                break;
        }
    }
}

public class Match
{
    public int playersInTeamA = 0;
    public int playersInTeamB = 0;
    public bool playerIsLeader;

    public Action onStartMatch;
}
