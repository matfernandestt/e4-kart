using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnerPlane : MonoBehaviour
{
    [SerializeField] private Renderer selfMesh;
    
    private void Awake()
    {
        if(selfMesh != null)
        {
#if UNITY_EDITOR
            selfMesh.enabled = true;
            return;
#endif
            selfMesh.enabled = false;
        }
    }
}
