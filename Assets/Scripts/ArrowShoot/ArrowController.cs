using UnityEngine;

public sealed class ArrowController : IService
{
    private readonly GameObject _root;
    private readonly ArrowView _arrowPrefab;
    private readonly ArrowPool<ArrowView> _arrowPool;

    public ArrowController(SpawnScriptableObject prefabContainer)
    {
        _root = new GameObject("Arrows");
        _arrowPrefab = prefabContainer.ArrowPrefab;
        _arrowPool = new ArrowPool<ArrowView>(_arrowPrefab, _root.transform);
    }

    public void ShootArrow(Transform placement, ArrowType arrowType, Vector3 target)
    {
        ArrowView arrow = _arrowPool.Spawn(placement, arrowType, target);
        arrow.OnHittingAgent += ReturnToPool;
    }

    private void ReturnToPool(ArrowView arrow)
    {
        arrow.OnHittingAgent -= ReturnToPool;
        _arrowPool.Despawn(arrow);
    }
}