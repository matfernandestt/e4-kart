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
    [SerializeField] private Toggle fpsToggle;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Scoreboard scoreboard;
    [SerializeField] private Button closePopupButton;
    [SerializeField] private Button leaveMatchButton;
    [SerializeField] private Button endMatchButton;
    [SerializeField] private Button quitGameButton;
    
    [SerializeField] private TextMeshProUGUI pingText;
    [SerializeField] private TextMeshProUGUI fpsText;

    private InputMapping mapping;

    public static Action<bool> onLockInputs;

    private void Awake()
    {
        mapping = new InputMapping();
        mapping.Enable();
        
        if(leaveMatchButton != null)
            leaveMatchButton.onClick.AddListener(LeaveMatch);
        if(endMatchButton != null)
            endMatchButton.onClick.AddListener(LeaveMatch);
        if(quitGameButton != null)
            quitGameButton.onClick.AddListener(QuitGame);
        
        bgmVolumeSlider.onValueChanged.AddListener(OnChangeVolumeBGM);
        sfxVolumeSlider.onValueChanged.AddListener(OnChangeVolumeSFX);

        mapping.Player.OpenMenu.performed += PerformOpenMenu;
        mapping.Player.Stats.performed += OpenStats;
        mapping.Player.Stats.canceled += CloseStats;
        
        closePopupButton.onClick.AddListener(() => OpenSettings(false));

        pingToggle.isOn = GlobalSettingsData.Instance.loadedSave.save.showPing;
        pingText.gameObject.SetActive(pingToggle.isOn);
        if (fpsToggle != null)
        {
            fpsToggle.isOn = GlobalSettingsData.Instance.loadedSave.save.showFPS;
            fpsText.gameObject.SetActive(fpsToggle.isOn);
        }

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

    public void OpenSettings(bool enable)
    {
        RefreshSliderValues();
        
        settingsMenu.SetActive(enable);
        onLockInputs?.Invoke(enable);

        if (!enable)
        {
            pingText.gameObject.SetActive(pingToggle.isOn);
            GlobalSettingsData.Instance.loadedSave.save.showPing = pingToggle.isOn;
            if (fpsToggle != null)
            {
                fpsText.gameObject.SetActive(fpsToggle.isOn);
                GlobalSettingsData.Instance.loadedSave.save.showFPS = fpsToggle.isOn;
            }

            SaveSystem.Save();
        }
    }

    private void RefreshSliderValues()
    {
        bgmVolumeSlider.value = GlobalSettingsData.Instance.Get_BGM_Volume();
        sfxVolumeSlider.value = GlobalSettingsData.Instance.Get_SFX_Volume();
    }

    private void OnChangeVolumeBGM(float value)
    {
        GlobalSettingsData.Instance.Set_BGM_Volume(bgmVolumeSlider.value);
    }
    
    private void OnChangeVolumeSFX(float value)
    {
        GlobalSettingsData.Instance.Set_SFX_Volume(sfxVolumeSlider.value);
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
        endMatchButton.interactable = false;
        PhotonNetwork.LeaveRoom();

        StartCoroutine(WaitToChangeScene());
        IEnumerator WaitToChangeScene()
        {
            Transitioner.Begin();
            yield return new WaitWhile(() => Transitioner.IsTransitioning);
            SceneManager.LoadScene("SCN_Lobby");
        }
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (PhotonNetwork.connected)
        {
            if(pingToggle.isOn)
                pingText.text = $"PING: {PhotonNetwork.GetPing()}ms";
        }
        
        if (fpsToggle != null)
        {
            if (fpsToggle.isOn)
                fpsText.text = $"FPS: {(int) (1 / Time.deltaTime)}";
        }
    }
}
