using System;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform reverseTarget;

    private bool reversing;
    private GlobalSettingsData globalRef;

    private void Awake()
    {
        transform.parent = null;
        
        globalRef = GlobalSettingsData.Instance;
    }

    private void FixedUpdate()
    {
        if (target == null || reverseTarget == null) return;

        if (!reversing)
        {
            transform.position = Vector3.Lerp(transform.position,
                target.position + globalRef.cameraPositionOffset,
                Time.deltaTime * globalRef.cameraFollowDelay);

            transform.forward = Vector3.Lerp(transform.forward,
                target.forward + globalRef.cameraRotationOffset,
                Time.deltaTime * globalRef.cameraFollowDelay);
        }
        else
        {
            transform.position = reverseTarget.position;
            transform.forward = reverseTarget.forward;
        }
    }

    public void UpdateReverseInput(bool isReversing)
    {
        reversing = isReversing;
    }

    public void ForceNormalCamera()
    {
        transform.position = target.position + globalRef.cameraPositionOffset;
        transform.forward = target.forward + globalRef.cameraRotationOffset;
    }
}
