using UnityEngine;

public class ArrowPool<T> : Pool<T>
    where T : MonoBehaviour, IArrow
{
    public ArrowPool(T prefab, Transform root) : base(prefab, root) { }

    public T Spawn(Transform placement, ArrowType arrowType, Vector3 target)
    {
        Vector3 direction = (target - placement.position).normalized;
        T result = base.Spawn();
        result.transform.position = placement.position;
        result.transform.rotation = Quaternion.LookRotation(direction);
        result.ArrowType = arrowType;
        result.RigidBody.velocity = direction * result.Force;
        return result;
    }
}