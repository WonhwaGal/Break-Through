using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    private Factory<T> _factory;
    private Stack<T> _storage;

    public Pool(T prefab)
    {
        _factory = new Factory<T>(prefab);
        _storage = new Stack<T>();
    }

    public T Spawn(Transform parent)
    {
        T sample;
        if (_storage.Count == 0)
            sample = _factory.Create();
        else
            sample = _storage.Pop();

        OnSpawn(sample, parent);
        return sample;
    }

    public void Despawn(T sample)
    {
        _storage.Push(sample);
        sample.gameObject.SetActive(false);
    }

    private void OnSpawn(T sample, Transform parent)
    {
        sample.gameObject.SetActive(true);
        sample.transform.parent = parent;
    }
}
