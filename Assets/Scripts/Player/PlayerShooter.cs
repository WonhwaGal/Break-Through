using UnityEngine;

public sealed class PlayerShooter : Shooter
{
    private readonly Pointer _aimPointer;
    private readonly Transform _playerT;
    private readonly float _shootLength;
    private float _stopShootingTime;
    private const float TransitionAdjustment = 0.8f;

    public PlayerShooter(Transform playerT, Transform bowShootPoint, float shootLength) : base(bowShootPoint)
    {
        _playerT = playerT;
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

        if(_aimPointer.PointerT.position.y <= 0)
            _targetPos = _bowShootPoint.position + _playerT.forward;
        else
            _targetPos = _aimPointer.PointerT.position;

        GameEventSystem.Send<ReceiveRewardEvent>(new ReceiveRewardEvent(RewardType.Arrow, -1, killReward: false));
        base.ShootArrow();
    }
}