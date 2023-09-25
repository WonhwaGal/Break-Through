using UnityEngine;

public class ArrowController
{
    private ArrowView _arrowPrefab;
    private Pool<ArrowView> _arrowPool;
    private GameObject _root;

    public ArrowController(SpawnScriptableObject prefabContainer)
    {
        _arrowPrefab = prefabContainer.ArrowPrefab;
        _root = new GameObject("Arrows");
        _arrowPool = new Pool<ArrowView>(_arrowPrefab);
    }

    public void PlaceArrow(Transform placement, ArrowType arrowType, Vector3 target)
    {
        ArrowView arrow = _arrowPool.Spawn(_root.transform);
        arrow.transform.position = placement.position;
        arrow.transform.rotation = placement.rotation;
        arrow.ArrowType = arrowType;
        Vector3 direction = (target - placement.position).normalized;
        arrow.ShootArrow(direction);
    }
}
