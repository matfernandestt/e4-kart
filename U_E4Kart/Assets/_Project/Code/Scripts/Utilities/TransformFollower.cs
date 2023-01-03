using UnityEngine;

public class TransformFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool position = true;
    [SerializeField] private bool rotation = true;
    [SerializeField] private bool scale = true;

    private void Update()
    {
        if (position)
        {
            transform.position = target.position;
        }

        if (rotation)
        {
            transform.rotation = target.rotation;
        }

        if (scale)
        {
            transform.localScale = target.localScale;
        }
    }
}
