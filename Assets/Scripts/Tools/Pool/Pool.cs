using System.Collections.Generic;
using UnityEngine;

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
        sample.transform.SetParent(RootObject);
    }
}