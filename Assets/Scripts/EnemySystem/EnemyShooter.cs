using UnityEngine;

public sealed class EnemyShooter : Shooter
{
    private const float VerticalShift = 2.8f;
    private const float AnimationShootTime = 1.2f;
    private float _timeToShoot;
    private Transform _targetAgent;
    private bool _getReadyToShoot;

    public EnemyShooter(Transform bowShootPoint) : base(bowShootPoint)
    {
        _arrowType = ArrowType.FromEnemy;
    }

    public void AssignTarget(Transform target)
    {
        _timeToShoot = Time.time + AnimationShootTime;
        _targetAgent = target;
        _getReadyToShoot = true;
    }

    public void Update()
    {
        if (_getReadyToShoot && _timeToShoot < Time.time)
        {
            _targetPos = _targetAgent.position;
            _getReadyToShoot = false;
            ShootArrow();
        }
    }

    public override void ShootArrow()
    {
        _targetPos += new Vector3(0, VerticalShift, 0);
        base.ShootArrow();
    }

    public void AdjustPauseTime(float deltaTime) => _timeToShoot += deltaTime;
}