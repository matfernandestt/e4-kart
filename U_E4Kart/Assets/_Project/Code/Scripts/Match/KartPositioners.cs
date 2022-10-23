using System;
using System.Collections.Generic;
using UnityEngine;

public class KartPositioners : MonoBehaviour
{
    [SerializeField] private Transform positionerPrefab;
    
    private List<Transform> positions = new List<Transform>();

    private void InitializePositioners()
    {
        positions = new List<Transform>();
        var quantity = GlobalSettingsData.Instance.maxPlayersPerRoom;

        for (var i = 0; i < quantity; i++)
        {
            var isEven = i % 2 == 0;
            var pos = transform.position + (new Vector3(4 * (isEven ? -1 : 1), 0, i * -4));
            var newPositioner = Instantiate(positionerPrefab, pos, transform.rotation, transform);
            positions.Add(newPositioner);
        }
    }

    public Transform GetPositionFromId(int positionId)
    {
        if(positions.Count == 0)
            InitializePositioners();
        return positionId < positions.Count ? positions[positionId] : positions[0];
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
