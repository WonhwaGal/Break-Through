using UnityEngine;

public class Factory<T> 
    where T : MonoBehaviour
{
    private readonly T _objectPrefab;

    public Factory(T prefab) => _objectPrefab = prefab;

    public T Create()
    {
        return GameObject.Instantiate(_objectPrefab);
    }
}
