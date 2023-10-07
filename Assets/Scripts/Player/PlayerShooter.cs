using UnityEngine;

public sealed class PlayerShooter : Shooter
{
    private readonly Pointer _aimPointer;
    private readonly float _shootLength;
    private float _stopShootingTime;
    private const float TransitionAdjustment = 0.8f;

    public PlayerShooter(Transform bowShootPoint, float shootLength) : base(bowShootPoint)
    {
        _aimPointer = ServiceLocator.Container.RequestFor<Pointer>();
        _shootLength = shootLength * TransitionAdjustment;
        _arrowType = ArrowType.FromPlayer;
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

    public override void ShootArrow()
    {
        _stopShootingTime = Time.time + _shootLength;

        _targetPos = _aimPointer.PointerT.position;
        if (_targetPos == Vector3.zero)
            _targetPos = _bowShootPoint.position + _bowShootPoint.forward;

        GameEventSystem.Send<ReceiveRewardEvent>(new ReceiveRewardEvent(RewardType.Arrow, -1, killReward: false));
        base.ShootArrow();
    }
}