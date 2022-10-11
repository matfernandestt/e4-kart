using UnityEngine;

[CreateAssetMenu(fileName = "AgentDataCollection", menuName = "Agents/Agent Data Collection")]
public class AgentDataCollection : ScriptableObject
{
    public AgentData[] gameAgents;
    
    public static int GetAgentId(AgentData agentToGet)
    {
        for (var i = 0; i < Instance.gameAgents.Length; i++)
        {
            var agent = Instance.gameAgents[i];
            if (agent == agentToGet)
                return i;
        }
        return 0;
    }

    public static AgentData GetAgentObject(int id)
    {
        var agent = Instance.gameAgents[0];
        if (Instance.gameAgents[id] != null)
        {
            agent = Instance.gameAgents[id];
        }

        return agent;
    }

    private static AgentDataCollection _instance;
    public static AgentDataCollection Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = Resources.Load<AgentDataCollection>("AgentDataCollection");
            return _instance;
        }
    }
}
