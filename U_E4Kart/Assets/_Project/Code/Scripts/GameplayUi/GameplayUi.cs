using System;
using Photon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUi : PunBehaviour
{
    [SerializeField] private ScrollRect logScrollView;
    [SerializeField] private TextMeshProUGUI logPrefab;

    public static Action<string> newLog;

    private void Awake()
    {
        ClearLogs();
        
        newLog += MatchLog;
    }

    private void OnDestroy()
    {
        newLog -= MatchLog;
    }

    public void ClearLogs()
    {
        foreach (Transform child in logScrollView.content)
        {
            Destroy(child.gameObject);
        }
    }

    private void MatchLog(string log)
    {
        var newLog = Instantiate(logPrefab, logScrollView.content);
        newLog.text = log;
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        base.OnPhotonPlayerDisconnected(otherPlayer);
        
        MatchLog($"{otherPlayer.NickName} has left the match!");
    }
}
