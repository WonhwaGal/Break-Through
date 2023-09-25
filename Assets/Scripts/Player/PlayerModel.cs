using System.Threading.Tasks;

public class PlayerModel
{
    private readonly PlayerAnimator _animator;
    private float _shootLength;
    private const float TransitionAdjustment = 0.8f;

    public PlayerModel(PlayerAnimator animator)
    {
        _animator = animator;
        _shootLength = _animator.GetLengthOfClip("PlayerShoot") * TransitionAdjustment;
    }

    public bool IsShooting { get; set; }

    public void StartAiming(bool aiming)
    {
        IsShooting = !aiming;

        if (IsShooting)
            WaitForEndOfShoot();
    }

    private async void WaitForEndOfShoot()
    {
        var length = (int)(_shootLength * 1000);
        await Task.Delay(length);

        IsShooting = false;
    }
}
