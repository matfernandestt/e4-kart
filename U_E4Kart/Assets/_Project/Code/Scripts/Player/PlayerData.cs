using ExitGames.Client.Photon;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    public string nickname;
    public PunTeams.Team team;

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
}
