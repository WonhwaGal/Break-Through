using System.Collections.Generic;
using UnityEngine;

//public class Test<T>   // decorator-proxy
//{
//    private Pool<T> _pool;

//    public Test(Pool<T> pool)
//    {
//        _pool = pool;
//    }

//    public T Spawn(int test1, GameObject x)
//    {
//        var result = _pool.Spawn();

//        ///&&&

//        return result;
//    }
//}


//public class ArowPool<T> : Pool<T>   // inheritance decorator
//{
//    ///
//    public T Spawn(int x, float force)
//    {
//        var result = base.Spawn();

//        ///&&&

//        return result;
//    }
//}
public class ArrowPool<T> : Pool<T>
    where T : MonoBehaviour, IArrow
{
    public ArrowPool(T prefab, Transform root) : base(prefab, root) { }

    public T Spawn(Transform placement, ArrowType arrowType, Vector3 target)
    {
        T result = base.Spawn();
        result.transform.position = placement.position;
        result.transform.rotation = Quaternion.LookRotation(target - placement.position, Vector3.up);
        result.ArrowType = arrowType;
        Vector3 direction = (target - placement.position).normalized;
        result.RigidBody.AddForce(direction * result.Force, ForceMode.Impulse);
        return result;
    }
}


public class Pool<T> : IPool<T>
    where T : MonoBehaviour
{
    private readonly Factory<T> _factory;
    private readonly Stack<T> _storage;
    private int _activeAgents = 0;

    public Pool(T prefab, Transform root)
    {
        _factory = new Factory<T>(prefab);
        _storage = new Stack<T>();
        RootObject = root;
    }

    public Transform RootObject { get; private set; }
    public int ActiveAgents { get => _activeAgents; }

    public T Spawn()
    {
        T sample;
        if (_storage.Count == 0)
            sample = _factory.Create();
        else
            sample = _storage.Pop();

        OnSpawn(sample);
        _activeAgents++;
        return sample;
    }

    public void Despawn(T sample)
    {
        _storage.Push(sample);
        sample.gameObject.SetActive(false);
        _activeAgents--;
    }

    private void OnSpawn(T sample)
    {
        sample.gameObject.SetActive(true);
        sample.transform.parent = RootObject;
    }
}