using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PublicityScreen : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button itchButton;
    [SerializeField] private Button twitterButton;
    
    private InputMapping mapping;
    
    private void Awake()
    {
        panel.SetActive(false);
        
        itchButton.onClick.AddListener(OpenItch);
        twitterButton.onClick.AddListener(OpenTwitter);
        
        mapping = new InputMapping();
        mapping.Enable();

        mapping.Player.Stats.performed += OpenScreen;
        mapping.Player.Stats.canceled += CloseScreen;
    }

    private void OnDestroy()
    {
        mapping.Player.Stats.performed -= OpenScreen;
        mapping.Player.Stats.canceled -= CloseScreen;
    }

    private void OpenScreen(InputAction.CallbackContext context)
    {
        panel.SetActive(true);
    }
    
    private void CloseScreen(InputAction.CallbackContext context)
    {
        panel.SetActive(false);
    }

    private void OpenItch()
    {
        Application.OpenURL("https://matfernandestt.itch.io/");
    }

    private void OpenTwitter()
    {
        Application.OpenURL("https://twitter.com/matfernandestt");
    }
}
