using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel
{
    private KillRewardType _rewardType;
    private int _rewardAmount;
    private const int MaxReward = 5;
    private bool _isDead;
    private bool _isIdle;

    public KillRewardType RewardType { get => _rewardType; }
    public int RewardAmount { get => _rewardAmount; }
    public bool IsIdle { get => _isIdle; set => _isIdle = value; }
    public Vector3 GuardPoint { get; set; }

    public void SetValues()
    {
        _rewardAmount = 1;

        int randomNumber = UnityEngine.Random.Range(1, 31);
        Array rewardTypeValues = Enum.GetValues(typeof(KillRewardType));
        _rewardType = (KillRewardType)rewardTypeValues.GetValue(randomNumber % rewardTypeValues.Length);

        if(_rewardType != KillRewardType.Key)
            _rewardAmount = randomNumber % MaxReward;
    }
}
