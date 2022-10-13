using System;
using UnityEngine;

public class KartPositioners : MonoBehaviour
{
    [SerializeField] private Transform[] positions;

    public Transform GetPositionFromId(int positionId)
    {
        return positionId < positions.Length ? positions[positionId] : positions[0];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (var position in positions)
        {
            if(position != null)
                Gizmos.DrawSphere(position.position, 1f);
        }
    }
}
