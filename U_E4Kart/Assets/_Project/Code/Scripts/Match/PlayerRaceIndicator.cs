using TMPro;
using UnityEngine;

public class PlayerRaceIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNickname;
    [SerializeField] private GameObject panel;
    
    private Camera mainCam;
    
    private void Awake()
    {
        mainCam = Camera.main;
        panel.SetActive(false);

        RaceController.onStartRace += RaceStart;
        RaceController.onFinishRace += RaceEnd;
    }

    private void OnDestroy()
    {
        RaceController.onStartRace -= RaceStart;
        RaceController.onFinishRace -= RaceEnd;
    }

    private void RaceStart()
    {
        if (!PhotonNetwork.connected) return;
        var playerKart = GetComponentInParent<PlayerKart>();
        if (playerKart != null)
        {
            if (playerKart.isMine)
            {
                Destroy(gameObject);
            }
            playerNickname.text = playerKart.photonPlayer.NickName;
            panel.SetActive(true);
        }
    }
    
    private void RaceEnd()
    {
        panel.SetActive(false);
    }

    private void Update()
    {
        if (mainCam == null) return;
        transform.forward = mainCam.transform.forward;
    }
}
