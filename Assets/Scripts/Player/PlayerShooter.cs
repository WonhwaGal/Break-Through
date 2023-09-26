using UnityEngine;

public class PlayerShooter
{
    private ArrowController _arrowController;
    private Pointer _aimPointer;
    private Transform _bowShootPoint;

    private float _shootLength;
    private float _stopShootingTime;
    private const float TransitionAdjustment = 0.8f;

    public PlayerShooter(Transform bowShootPoint, float shootLength)
    {
        _arrowController = ServiceLocator.Container.RequestFor<ArrowController>();
        _aimPointer = ServiceLocator.Container.RequestFor<Pointer>();
        _bowShootPoint = bowShootPoint;
        _shootLength = shootLength * TransitionAdjustment;
    }

    public bool IsShooting
    {
        get
        {
            if (_stopShootingTime > Time.time)
                return true;
            return false;
        }
    }

    public void ShootArrow()
    {
        _stopShootingTime = Time.time + _shootLength;

        var pointerPos = _aimPointer.PointerT.position;
        if (pointerPos == Vector3.zero)
            pointerPos = _bowShootPoint.position + _bowShootPoint.forward;

        _arrowController.ShootArrow(_bowShootPoint, ArrowType.FromPlayer, pointerPos);
    }
}