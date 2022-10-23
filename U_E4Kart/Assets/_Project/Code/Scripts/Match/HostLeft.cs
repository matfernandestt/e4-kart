using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HostLeft : MonoBehaviour
{
    [SerializeField] private Button returnToLobbyButton;

    private void Awake()
    {
        returnToLobbyButton.onClick.AddListener(ReturnToLobby);
        Transitioner.FadeOut();
    }

    private void ReturnToLobby()
    {
        returnToLobbyButton.interactable = false;
        
        StartCoroutine(WaitToChangeScene());
        IEnumerator WaitToChangeScene()
        {
            Transitioner.Begin();
            yield return new WaitWhile(() => Transitioner.IsTransitioning);
            SceneManager.LoadScene("SCN_Lobby");
        }
    }
}
