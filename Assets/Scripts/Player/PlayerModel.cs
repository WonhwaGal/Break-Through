using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class PlayerModel : IDisposable
{
    private int _hp;
    private const float DrownControl = -9.15f;
    private readonly PlayerAnimator _animator;
    private readonly PlayerShooter _shooter;
    private readonly int _damageFromArrow;
    private readonly Transform _transform;
    private readonly StatisticsCounter _stats;

    public PlayerModel(PlayerAnimator animator, Transform bowShootPoint, Transform playerT)
    {
        _animator = animator;
        _transform = playerT;
        _damageFromArrow = SceneManager.GetActiveScene().buildIndex == 3 ? 
            _damageFromArrow = Constants.BossDamageToPlayer : _damageFromArrow = Constants.ArrowDamageToPlayer;
        _shooter = new PlayerShooter(_transform, bowShootPoint, _animator.GetLengthOfClip("PlayerShoot"));
        _stats = ServiceLocator.Container.RequestFor<StatisticsCounter>();
        _hp = _stats.PlayerHP;

        GameEventSystem.Subscribe<PlayerAimEvent>(StartAiming);
        GameEventSystem.Subscribe<SaveGameEvent>(SavePlayerData);
        GameEventSystem.Subscribe<StatsChangedEvent>(AddHP);
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

    public void StartAiming(PlayerAimEvent @event)
    {
        IsShooting = !@event.AimPressed;

        if (_stats.ArrowNumber <= 0)
            return;

        if (IsShooting && !IsHurting)
            _shooter.ShootArrow();
    }

    public bool ShouldStand()
    {
        if (_transform.position.y < DrownControl)
            HP = 0;

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
            GameEventSystem.Send<GameStopEvent>(new GameStopEvent(false, isEnded: true, isWin: false));
        }
        else
        {
            _animator.AnimateDamage();
        }
        GameEventSystem.Send<PlayerHpEvent>(new PlayerHpEvent(_hp, oldHp, IsHurting));
    }

    private void AddHP(StatsChangedEvent @event)
    {
        if(@event.RewardType == RewardType.HP)
            _hp = @event.NewValue;
    }

    private void SavePlayerData(SaveGameEvent @event)
    {
        var progressData = ServiceLocator.Container.RequestFor<ProgressData>();
        if (@event.ProgressToSave)
            progressData.PlayerPos = _transform.position;
    }

    public void Dispose()
    {
        GameEventSystem.UnSubscribe<PlayerAimEvent>(StartAiming);
        GameEventSystem.UnSubscribe<SaveGameEvent>(SavePlayerData);
        GameEventSystem.UnSubscribe<StatsChangedEvent>(AddHP);
        _stats.Dispose();
        GC.SuppressFinalize(this);
    }
}