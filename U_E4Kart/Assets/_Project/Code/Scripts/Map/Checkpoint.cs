using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public void FixPosition()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, 10f))
        {
            var road = hit.collider.GetComponent<Road>();
            if (road != null)
            {
                transform.position = hit.point;
                transform.up = hit.normal;
            }
        }
    }

    private void OnDrawGizmos()
    {
        var c = Color.green;
        c.a = .3f;
        Gizmos.color = c;
        Gizmos.DrawSphere(transform.position, transform.localScale.x);
    }
}
