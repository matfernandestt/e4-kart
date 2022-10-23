using System;
using System.Collections;
using Photon;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostLeftMatchWatcher : PunBehaviour
{
    private PhotonPlayer roomLeader;

    private void Awake()
    {
        roomLeader = PlayerData.GetLeader();
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        base.OnPhotonPlayerDisconnected(otherPlayer);
        
        if(otherPlayer == roomLeader)
            DisconnectProcess();
    }

    private void DisconnectProcess()
    {
        PhotonNetwork.LeaveRoom();
        
        StartCoroutine(Transition());
        IEnumerator Transition()
        {
            Transitioner.FadeIn();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("SCN_HostLeft");
        }
    }
}
