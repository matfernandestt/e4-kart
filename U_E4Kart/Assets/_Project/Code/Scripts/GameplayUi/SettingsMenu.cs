using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;
    
    [SerializeField] private Slider sensitivitySlider;
    
    private InputMapping mapping;

    public static Action<bool> onLockInputs;

    private void Awake()
    {
        mapping = new InputMapping();
        mapping.Enable();

        mapping.Player.OpenMenu.performed += PerformOpenMenu;
        
        LockCursor(true);

        sensitivitySlider.value = GlobalSettingsData.Instance.mouseSensitivity;
    }

    private void OnDestroy()
    {
        mapping.Player.OpenMenu.performed -= PerformOpenMenu;
    }

    private void PerformOpenMenu(InputAction.CallbackContext context)
    {
        OpenSettings(!settingsMenu.activeSelf);
    }

    private void OpenSettings(bool enable)
    {
        settingsMenu.SetActive(enable);
        LockCursor(!enable);
        onLockInputs?.Invoke(enable);

        if (!enable)
        {
            GlobalSettingsData.Instance.mouseSensitivity = sensitivitySlider.value;
        }
    }
    
    public static void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        //Cursor.visible = locked;
    }
}
