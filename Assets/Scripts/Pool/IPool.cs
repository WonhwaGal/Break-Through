using UnityEngine;

public interface IPool<T>
{
    int ActiveAgents { get; }
    T Spawn();
    void Despawn(T sample);
}
