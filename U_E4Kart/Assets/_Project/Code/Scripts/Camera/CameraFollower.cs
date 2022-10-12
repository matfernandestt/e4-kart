using System;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform target;

    private GlobalSettingsData globalRef;

    private void Awake()
    {
        globalRef = GlobalSettingsData.Instance;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, 
            target.position + globalRef.cameraPositionOffset,
            Time.deltaTime * globalRef.cameraFollowDelay);

        transform.forward = Vector3.Lerp(transform.forward,
            target.forward + globalRef.cameraRotationOffset,
            Time.deltaTime * globalRef.cameraFollowDelay);
    }
}
