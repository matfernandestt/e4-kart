using UnityEngine;
using UnityEngine.UI;

public class SelectableAgentBase : MonoBehaviour
{
    [SerializeField] private CharacterData characterData;
    [Space]
    [SerializeField] private Toggle selfButton;
    [SerializeField] private Image agentIcon;

    private CharacterSelection parent;

    private void Awake()
    {
        parent = GetComponentInParent<CharacterSelection>(true);
        
        agentIcon.sprite = characterData.characterIcon;
        selfButton.onValueChanged.AddListener(OnSelectAgent);
    }

    private void OnSelectAgent(bool value)
    {
        if (!value) return;
        parent.DisableAll(gameObject);
        
        CharacterSelection.onLocalPlayerSelectAgent?.Invoke(characterData);
        selfButton.interactable = false;
    }

    public void DisableToggle()
    {
        selfButton.isOn = false;
        selfButton.interactable = true;
    }
}
