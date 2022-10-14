using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Toggle pingToggle;
    [SerializeField] private Scoreboard scoreboard;
    [SerializeField] private Button leaveMatchButton;
    
    [SerializeField] private TextMeshProUGUI pingText;

    private InputMapping mapping;

    public static Action<bool> onLockInputs;

    private void Awake()
    {
        mapping = new InputMapping();
        mapping.Enable();
        
        leaveMatchButton.onClick.AddListener(LeaveMatch);

        mapping.Player.OpenMenu.performed += PerformOpenMenu;
        mapping.Player.Stats.performed += OpenStats;
        mapping.Player.Stats.canceled += CloseStats;

        pingText.gameObject.SetActive(pingToggle.isOn);
        scoreboard.gameObject.SetActive(false);
        
        scoreboard.InitializeScoreboard(PhotonNetwork.connected ? PhotonNetwork.playerList : null);
    }

    private void OnDestroy()
    {
        mapping.Player.OpenMenu.performed -= PerformOpenMenu;
        mapping.Player.Stats.performed -= OpenStats;
        mapping.Player.Stats.canceled -= CloseStats;
    }

    private void PerformOpenMenu(InputAction.CallbackContext context)
    {
        OpenSettings(!settingsMenu.activeSelf);
    }

    private void OpenSettings(bool enable)
    {
        settingsMenu.SetActive(enable);
        onLockInputs?.Invoke(enable);

        if (!enable)
        {
            pingText.gameObject.SetActive(pingToggle.isOn);
        }
    }

    private void OpenStats(InputAction.CallbackContext context)
    {
        scoreboard.gameObject.SetActive(true);

        if (PhotonNetwork.connected)
        {
            scoreboard.UpdateScoreboard(PhotonNetwork.playerList);
        }
    }

    private void CloseStats(InputAction.CallbackContext context)
    {
        scoreboard.gameObject.SetActive(false);
    }

    private void LeaveMatch()
    {
        leaveMatchButton.interactable = false;
        PhotonNetwork.LeaveLobby();

        StartCoroutine(WaitToChangeScene());
        IEnumerator WaitToChangeScene()
        {
            Transitioner.Begin();
            yield return new WaitWhile(() => Transitioner.IsTransitioning);
            SceneManager.LoadScene("SCN_Lobby");
        }
    }

    private void Update()
    {
        if (PhotonNetwork.connected)
        {
            pingText.text = $"PING: {PhotonNetwork.GetPing()}ms";
        }
    }
}
