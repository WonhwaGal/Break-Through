using UnityEngine;

public class PlayerModel
{
    private readonly PlayerAnimator _animator;
    private readonly PlayerShooter _shooter;

    public PlayerModel(PlayerAnimator animator, Transform bowShootPoint)
    {
        _animator = animator;
        _shooter = new PlayerShooter(bowShootPoint, _animator.GetLengthOfClip("PlayerShoot"));
    }

    public bool IsShooting { get; private set; }
    public bool IsstaticShooting => _shooter.IsShooting;

    public void StartAiming(bool aiming)
    {
        IsShooting = !aiming;

        if (IsShooting)
            _shooter.ShootArrow();
    }
}
