using System;
using TMPro;
using UnityEngine;

public class RaceEndScreen : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI winnerText;

    private void Awake()
    {
        panel.SetActive(false);

        RaceController.onFinishRace += SetGameEnd;
    }

    private void OnDestroy()
    {
        RaceController.onFinishRace -= SetGameEnd;
    }

    private void SetGameEnd()
    {
        panel.SetActive(true);

        var winner = RaceController.GetCustomProperty(RaceController.CustomProperty_RaceWinner);
        winnerText.text = $"CONGRATULATION!\n THE WINNER IS {(string)winner}";
    }
}
