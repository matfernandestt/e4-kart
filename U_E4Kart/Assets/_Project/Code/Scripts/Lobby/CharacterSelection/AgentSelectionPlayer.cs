using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentSelectionPlayer : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    
    [SerializeField] private Image agentIcon;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI agentName;
    [SerializeField] private GameObject isThisPlayerYou;

    public void Setup(string player, CharacterData character)
    {
        playerName.text = player;
        SetAgent(character);

        canvasGroup.alpha = 1;
    }

    public void SetLocalPlayer()
    {
        isThisPlayerYou.SetActive(true);
    }

    public void SetNoPlayer()
    {
        canvasGroup.alpha = 0;
    }

    public void SetAgent(CharacterData data)
    {
        agentIcon.sprite = data.characterIcon;
        agentName.text = data.characterName;
    }
}
