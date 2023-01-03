using System;
using TMPro;
using UnityEngine;

public class RaceEndScreen : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private AudioClip winBgm;
    [SerializeField] private AudioClip loseBgm;

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
        var isWinner = (string) winner == PhotonNetwork.player.NickName;
        var color = isWinner ? Color.green : Color.red;
        var parsedColor = ColorUtility.ToHtmlStringRGB(color);
        winnerText.text = $"<color=#{parsedColor}>{(string)winner}</color>";
        BgmManager.PlayBGM(isWinner ? winBgm : loseBgm);
    }
}
