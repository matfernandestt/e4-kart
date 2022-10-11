using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectableAgentBase : MonoBehaviour
{
    [SerializeField] private AgentData agentData;
    [Space]
    [SerializeField] private Button selfButton;
    [SerializeField] private Image agentIcon;

    private void Awake()
    {
        agentIcon.sprite = agentData.agentIcon;
        selfButton.onClick.AddListener(OnSelectAgent);
    }

    private void OnSelectAgent()
    {
        AgentSelection.onLocalPlayerSelectAgent?.Invoke(agentData);
    }
}
