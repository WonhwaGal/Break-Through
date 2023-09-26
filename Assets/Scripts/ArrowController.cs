using UnityEngine;

public class ArrowController : IService
{
    private ArrowView _arrowPrefab;
    private ArrowPool<ArrowView> _arrowPool;
    private GameObject _root;

    public ArrowController(SpawnScriptableObject prefabContainer)
    {
        _arrowPrefab = prefabContainer.ArrowPrefab;
        _root = new GameObject("Arrows");
        _arrowPool = new ArrowPool<ArrowView>(_arrowPrefab, _root.transform);
    }

    public void ShootArrow(Transform placement, ArrowType arrowType, Vector3 target)
    {
        _arrowPool.Spawn(placement, arrowType, target);
    }
}