using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectableAgentBase : MonoBehaviour
{
    [FormerlySerializedAs("agentData")] [SerializeField] private CharacterData characterData;
    [Space]
    [SerializeField] private Button selfButton;
    [SerializeField] private Image agentIcon;

    private void Awake()
    {
        agentIcon.sprite = characterData.characterIcon;
        selfButton.onClick.AddListener(OnSelectAgent);
    }

    private void OnSelectAgent()
    {
        AgentSelection.onLocalPlayerSelectAgent?.Invoke(characterData);
    }
}
