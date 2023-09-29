using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyModel :IDisposable
{
    private int _hp;
    private KillRewardType _rewardType;
    private int _rewardAmount;
    private Transform _target;
    private bool _isDead;
    private bool _isIdle;
    private bool _isShooting;
    private const int MaxRewardValue = 5;
    private const float ChaseSpan = 3.0f;
    private const float StayAfterDeathSpan = 5.0f;
    private readonly EnemyShooter _shooter;
    private readonly int _damageFromArrow;
    private readonly Slider _hpSlider;

    public EnemyModel(Transform shootPoint, int damage, Slider hpSlider)
    {
        _shooter = new EnemyShooter(shootPoint);
        _damageFromArrow = damage;
        _hpSlider = hpSlider;
        _hpSlider.onValueChanged.AddListener(UpdateSlider);
        GameEventSystem.Subscribe<GameStopEvent>(GameStopped);
    }

    public KillRewardType RewardType { get => _rewardType; }
    public int RewardAmount { get => _rewardAmount; }
    public Vector3 GuardPoint { get; set; }
    public Type State { get; set; }
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

            OnMoving?.Invoke(_target != null, true, State);
        }
    }
    public bool IsIdle
    {
        get => _isIdle;
        set
        {
            _isIdle = value;
            if (value)
                OnMoving?.Invoke(_target != null, false, State);
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
    public float ChaseTimeSpan => ChaseSpan;
    public float StayAfterDeathTime => StayAfterDeathSpan;

    public event Action<bool> OnSeeingPlayer;
    public event Action<bool, bool, Type> OnMoving;
    public event Action OnStartShooting;
    public event Action OnDying;
    public event Action<EnemyView> OnReadyToDespawn;


    public void Reset()
    {
        _hpSlider.gameObject.SetActive(true);
        HP = (int)_hpSlider.maxValue;

        SetRewardValues();
    }

    private void SetRewardValues()
    {
        _rewardAmount = 1;

        int randomNumber = UnityEngine.Random.Range(1, 31);
        Array rewardTypeValues = Enum.GetValues(typeof(KillRewardType));
        _rewardType = (KillRewardType)rewardTypeValues.GetValue(randomNumber % rewardTypeValues.Length);

        if(_rewardType != KillRewardType.Key)
            _rewardAmount = randomNumber % MaxRewardValue;
    }

    public void CauseDamage(ArrowType arrowType)
    {
        if (arrowType == ArrowType.FromPlayer)
            HP -= _damageFromArrow;
    }

    public void UpdateShooter() => _shooter.Update();
    public void InvokeDespawn(EnemyView enemy) => OnReadyToDespawn?.Invoke(enemy);
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