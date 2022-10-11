using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentSelectionPlayer : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    
    [SerializeField] private Image agentIcon;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI agentName;

    public void Setup(string player, AgentData agent)
    {
        playerName.text = player;
        SetAgent(agent);

        canvasGroup.alpha = 1;
    }

    public void SetNoPlayer()
    {
        canvasGroup.alpha = 0;
    }

    public void SetAgent(AgentData data)
    {
        agentIcon.sprite = data.agentIcon;
        agentName.text = data.agentName;
    }
}
