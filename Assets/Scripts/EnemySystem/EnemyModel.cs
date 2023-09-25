using System;
using UnityEngine;

public class EnemyModel
{
    private int _hp;
    private KillRewardType _rewardType;
    private int _rewardAmount;
    private Transform _target;
    private bool _isDead;
    private bool _isIdle;
    private bool _isShooting;
    private const int MaxRewardValue = 5;
    private const int ChaseSpanInMillSec = 3000;

    public KillRewardType RewardType { get => _rewardType; }
    public int RewardAmount { get => _rewardAmount; }
    public Vector3 GuardPoint { get; set; }
    public bool IsIdle
    {
        get => _isIdle;
        set
        {
            _isIdle = value;
            if (value)
                OnMoving?.Invoke(false);
        }
    }
    public bool IsShooting
    {
        get => _isShooting;
        set
        {
            _isShooting = value;
            if (value)
                OnTakingAShot?.Invoke();
        }
    }
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

            OnMoving?.Invoke(true);
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
    public int HP { get => _hp; set => _hp = value; }
    public int ChaseSpanInMilliSec => ChaseSpanInMillSec;

    public event Action<bool> OnSeeingPlayer;
    public event Action<bool> OnMoving;
    public event Action OnTakingAShot;
    public event Action OnDying;

    public void SetRewardValues()
    {
        _rewardAmount = 1;

        int randomNumber = UnityEngine.Random.Range(1, 31);
        Array rewardTypeValues = Enum.GetValues(typeof(KillRewardType));
        _rewardType = (KillRewardType)rewardTypeValues.GetValue(randomNumber % rewardTypeValues.Length);

        if(_rewardType != KillRewardType.Key)
            _rewardAmount = randomNumber % MaxRewardValue;
    }
}