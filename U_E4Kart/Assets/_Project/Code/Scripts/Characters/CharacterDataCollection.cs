using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataCollection", menuName = "Characters/Character Data Collection")]
public class CharacterDataCollection : ScriptableObject
{
    public CharacterData[] gameAgents;
    
    public static int GetAgentId(CharacterData characterToGet)
    {
        for (var i = 0; i < Instance.gameAgents.Length; i++)
        {
            var agent = Instance.gameAgents[i];
            if (agent == characterToGet)
                return i;
        }
        return 0;
    }

    public static CharacterData GetAgentObject(int id)
    {
        var agent = Instance.gameAgents[0];
        if (Instance.gameAgents[id] != null)
        {
            agent = Instance.gameAgents[id];
        }

        return agent;
    }

    private static CharacterDataCollection _instance;
    public static CharacterDataCollection Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = Resources.Load<CharacterDataCollection>("CharacterDataCollection");
            return _instance;
        }
    }
}
