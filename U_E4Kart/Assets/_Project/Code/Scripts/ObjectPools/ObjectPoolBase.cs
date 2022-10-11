using UnityEngine;

public class ObjectPoolBase : MonoBehaviour
{
    [SerializeField] protected int spawnQuantity;

    public virtual void SpawnObjects() { }
}
