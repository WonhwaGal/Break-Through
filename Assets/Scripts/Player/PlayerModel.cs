using UnityEngine;

public class PlayerModel
{
    private int _hp;
    private const int _maxHp = 100;
    private readonly PlayerAnimator _animator;
    private readonly PlayerShooter _shooter;
    private readonly int _damageFromArrow;

    public PlayerModel(PlayerAnimator animator, Transform bowShootPoint, int damage)
    {
        _animator = animator;
        _damageFromArrow = damage;
        _shooter = new PlayerShooter(bowShootPoint, _animator.GetLengthOfClip("PlayerShoot"));
        GameEventSystem.Subscribe<PlayerAimEvent>(StartAiming);
        Reset();
    }

    public bool IsHurting => _animator.CheckCurrentClip("Hurt-Reaction");
    public bool IsDead { get; private set; }
    public bool IsShooting { get; private set; }
    public int HP
    {
        get => _hp;
        private set
        {
            if (_hp > value)
                HpDown(value, _hp);
            else
                _hp = value;
        }
    }

    public void Reset() => HP = _maxHp;

    public void StartAiming(PlayerAimEvent @event)
    {
        IsShooting = !@event.AimPressed;

        if (IsShooting && !IsHurting)
            _shooter.ShootArrow();
    }

    public bool ShouldStand()
    {
        if (IsHurting || _shooter.IsShooting)
            return true;
        return false;
    }

    public void CauseDamage(ArrowType arrowType)
    {
        if (arrowType == ArrowType.FromPlayer || IsDead)
            return;

        HP -= _damageFromArrow;
    }

    private void HpDown(int value, int oldHp)
    {
        _hp = value;
        if (_hp <= 0)
        {
            _hp = 0;
            IsDead = true;
            _animator.AnimateDeath();
            GameEventSystem.Send<GameStopEvent>(new GameStopEvent(isEnded: true, isPaused: false));
        }
        else
        {
            _animator.AnimateDamage();
        }
        var isHurting = IsHurting;
        GameEventSystem.Send<PlayerHpEvent>(new PlayerHpEvent(_hp, oldHp, isHurting));
    }
}