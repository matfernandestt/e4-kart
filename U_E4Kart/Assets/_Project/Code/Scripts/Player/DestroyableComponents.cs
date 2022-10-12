using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DestroyableComponents : MonoBehaviour
{
    public Component[] componentsToDestroy;
    public GameObject[] gameObjectsToDestroy;

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
