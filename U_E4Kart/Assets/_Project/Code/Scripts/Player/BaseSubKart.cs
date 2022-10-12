using System;
using UnityEngine;

public class BaseSubKart : MonoBehaviour
{
    [SerializeField] private Transform collider;

    public Transform GetColliderTransform => collider;
    
    public void UpdateRotation(Vector3 axisInput)
    {
        transform.Rotate(new Vector3(0, axisInput.x, 0) * Time.deltaTime * 50f);
    }
}
