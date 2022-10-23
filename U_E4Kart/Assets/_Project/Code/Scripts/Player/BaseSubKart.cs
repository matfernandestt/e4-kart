using System;
using UnityEngine;

public class BaseSubKart : MonoBehaviour
{
    [SerializeField] private Transform collider;
    [SerializeField] private KartVisualController visuals;

    public Transform GetColliderTransform => collider;
    public KartVisualController GetVisuals => visuals;
    
    public void UpdateRotation(Vector3 axisInput)
    {
        transform.Rotate(new Vector3(0, axisInput.x, 0) * Time.deltaTime * 50f);
    }
}
