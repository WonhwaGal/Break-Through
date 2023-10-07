using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class EnemyModel :IDisposable
{
    private int _hp;
    private RewardType _rewardType;
    private int _rewardAmount;
    private Transform _target;
    private bool _isDead;
    private bool _isIdle;
    private bool _isShooting;
    private readonly EnemyShooter _shooter;
    private readonly int _damageFromArrow;
    private readonly Slider _hpSlider;
    private float _stayAfterDeath = 5.0f;

    public EnemyModel(Transform shootPoint, Slider hpSlider, int damage, IStateMachine stateMachine)
    {
        _hpSlider = hpSlider;
        StateMachine = stateMachine;
        _shooter = new EnemyShooter(shootPoint);
        _damageFromArrow = damage;
        _hpSlider.onValueChanged.AddListener(UpdateSlider);
        GameEventSystem.Subscribe<GameStopEvent>(GameStopped);
    }

    public RewardType RewardType { get => _rewardType; }
    public int RewardAmount { get => _rewardAmount; }
    public IStateMachine StateMachine { get; private set; }
    public float StayAfterDeathTime
    {
        get => _stayAfterDeath;
        set => _stayAfterDeath = value;
    }
    public Vector3 GuardPoint { get; set; }
    public bool IsPaused { get; set; }
    public Transform Target
    {
        get => _target;
        set
        {
            _target = value;
            if (_target != null)
                OnSeeingPlayer?.Invoke(true);
            else
                OnSeeingPlayer?.Invoke(false);

            OnMoving?.Invoke(_target != null, true, StateMachine.CurrentState);
        }
    }
    public bool IsIdle
    {
        get => _isIdle;
        set
        {
            _isIdle = value;
            if (value)
                OnMoving?.Invoke(_target != null, false, StateMachine.CurrentState);
        }
    }
    public bool IsShooting
    {
        get => _isShooting;
        set
        {
            _isShooting = value;
            if (value)
            {
                OnStartShooting?.Invoke();
                _shooter.AssignTarget(_target);
            }
        }
    }
    public bool IsDead 
    { 
        get => _isDead;
        set
        {
            _isDead = value;
            if (value)
                OnDying?.Invoke();
        }
    }
    public int HP
    { 
        get => _hp;
        set
        {
            _hp = value;
            if (value <= 0)
                IsDead = true;
            _hpSlider.value = _hp;
        }
    }

    public event Action<bool, bool, IState> OnMoving;
    public event Action<bool> OnSeeingPlayer;
    public event Action OnStartShooting;
    public event Action OnDying;
    public event Action<EnemyView> OnReadyToDespawn;

    public void Reset()
    {
        _hpSlider.gameObject.SetActive(true);
        HP = (int)_hpSlider.maxValue;

        IsDead = false;
        SetRewardValues();
    }

    private void SetRewardValues()
    {
        int randomNumber = UnityEngine.Random.Range(1, 31);

        _rewardAmount = randomNumber % Constants.KillRewardMaxValue;
        if (_rewardAmount == 0 || _rewardType == RewardType.Key)
            _rewardAmount = 1;

        Array rewardTypeValues = Enum.GetValues(typeof(RewardType));
        for (var i = rewardTypeValues.Length - 1; i >= 0; i--)
        {
            if (randomNumber % (int)rewardTypeValues.GetValue(i) == 0)
            {
                _rewardType = (RewardType)rewardTypeValues.GetValue(i);
                return;
            }
        }
    }

    public void CauseDamage(ArrowType arrowType)
    {
        if (arrowType == ArrowType.FromPlayer)
            HP -= _damageFromArrow;
    }

    public void UpdateShooter() => _shooter.Update();
    public void AdjustShooter(float deltaTime) => _shooter.AdjustPauseTime(deltaTime);
    public void InvokeDespawn(EnemyView enemy) => OnReadyToDespawn?.Invoke(enemy);
    public void Pause(GameStopEvent @event) => IsPaused = @event.IsPaused;
    private void UpdateSlider(float value) => _hpSlider.gameObject.SetActive(value > 0);

    private void GameStopped(GameStopEvent @event)
    {
        if (@event.EndOfGame)
            Target = null;
    }

    public void Dispose()
    {
        _hpSlider.onValueChanged.RemoveAllListeners();
        GameEventSystem.UnSubscribe<GameStopEvent>(GameStopped);
        GC.SuppressFinalize(this);
    }
}