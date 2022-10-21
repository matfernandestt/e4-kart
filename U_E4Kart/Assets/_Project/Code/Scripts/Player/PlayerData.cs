using ExitGames.Client.Photon;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    public string nickname;
    public PunTeams.Team team;

    public const string CustomProperty_IsRoomLeader = "isRoomLeader";
    public const string CustomProperty_SelectedMap = "selectedMap";

    public static void SetCustomProperty(PhotonPlayer player, string customPropertyName, object value)
    {
        if (player.CustomProperties.ContainsKey(customPropertyName))
        {
            player.CustomProperties.Remove(customPropertyName);
        }
        
        var hash = new Hashtable();
        hash.Add(customPropertyName, value);
        player.SetCustomProperties(hash);
    }

    public static object GetCustomProperty(PhotonPlayer player, string customPropertyName)
    { 
        return player.CustomProperties[customPropertyName];
    }

    public static PhotonPlayer GetLeader()
    {
        var players = PhotonNetwork.playerList;
        foreach (var player in players)
        {
            var customProperty = player.CustomProperties[CustomProperty_IsRoomLeader];
            if (customProperty != null)
            {
                if ((bool) player.CustomProperties[CustomProperty_IsRoomLeader])
                {
                    return player;
                }
            }
        }
        return null;
    }
}
