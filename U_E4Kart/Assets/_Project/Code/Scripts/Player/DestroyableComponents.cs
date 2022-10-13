using System;
using Photon;
using UnityEngine;

public class DestroyableComponents : PunBehaviour
{
    public Component[] componentsToDestroy;
    public GameObject[] gameObjectsToDestroy;

    private void Awake()
    {
        if(PhotonNetwork.connected)
            DestroyComponents(photonView.isMine);
    }

    public void DestroyComponents(bool isMine)
    {
        if (!isMine)
        {
            foreach (var component in componentsToDestroy)
            {
                Destroy(component);
            }

            foreach (var obj in gameObjectsToDestroy)
            {
                Destroy(obj);
            }
        }
        
        Destroy(this);
    }
}
