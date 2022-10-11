using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    [SerializeField] private Button selfButton;
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TextMeshProUGUI playersInRoomText;
    [SerializeField] private Image roomAvailability;

    public void SetupButton(string name, int playersInRoom, int maxPlayersInRoom, Action onClickButton = null)
    {
        roomName.text = name;
        playersInRoomText.text = $"{playersInRoom}/{maxPlayersInRoom}";
        selfButton.onClick.AddListener(() => onClickButton?.Invoke());

        var fullRoom = playersInRoom == maxPlayersInRoom;
        roomAvailability.color = fullRoom ? Color.red : Color.green;
        selfButton.interactable = !fullRoom;
    }
}
