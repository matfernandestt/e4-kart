using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private Transform parent;
    
    public Quaternion Normal { private set; get; }
    
    public bool IsGrounded { private set; get; }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, 1f))
        {
            var road = hit.collider.GetComponent<Road>();
            if (road != null)
            {
                IsGrounded = true;

                Normal = Quaternion.FromToRotation(parent.up, hit.normal) * parent.rotation;
                parent.position = new Vector3(parent.position.x, hit.point.y, parent.position.z);
            }
            else
            {
                IsGrounded = false;
            }
        }
        else
        {
            IsGrounded = false;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * 1f));
    }
}
