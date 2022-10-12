using UnityEngine;

public class Road : MonoBehaviour
{
    public void RefreshCollision()
    {
        var currentCol = GetComponent<MeshCollider>();
        if (currentCol != null)
        {
            DestroyImmediate(currentCol);
        }
        gameObject.AddComponent<MeshCollider>();
    }
}
