using UnityEngine;

public abstract class Shooter
{
    protected readonly ArrowController _arrowController;
    protected readonly Transform _bowShootPoint;
    protected ArrowType _arrowType;
    protected Vector3 _targetPos;

    public Shooter(Transform bowShootPoint)
    {
        _bowShootPoint = bowShootPoint;
        _arrowController = ServiceLocator.Container.RequestFor<ArrowController>();
    }

    public virtual void ShootArrow()
    {
        _arrowController.ShootArrow(_bowShootPoint, _arrowType, _targetPos);
    }
}