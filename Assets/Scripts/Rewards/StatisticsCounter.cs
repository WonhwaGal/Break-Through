using System;
using UnityEngine;

public sealed class StatisticsCounter : IService, IDisposable
{
    private const int SliderMultiplier = 10;
    private const int MaxSliderValue = 100;
    private int _arrowNumber;

    public StatisticsCounter()
    {
        GameEventSystem.Subscribe<ReceiveRewardEvent>(UpdateStats);
        GameEventSystem.Subscribe<SaveGameEvent>(SaveProgress);
        GameEventSystem.Subscribe<PlayerHpEvent>(SetHpSlider);
    }

    public int ArrowNumber
    {
        get => _arrowNumber;
        set
        {
            _arrowNumber = value;
            PlayerPrefs.SetInt(Constants.CurrentArrowNumber, _arrowNumber);
        }
    }
    public int KeyNumber { get; private set; }
    public int EnemyNumber { get; private set; }
    public int PlayerHP { get; private set; } = MaxSliderValue;

    private void UpdateStats(ReceiveRewardEvent @event)
    {
        if(@event.KillReward)
            EnemyNumber++;

        switch (@event.RewardType)
        {
            case RewardType.Arrow:
                ArrowNumber = UpdateValue(ArrowNumber, @event);
                break;
            case RewardType.Key:
                KeyNumber = UpdateValue(KeyNumber, @event);
                break;
            default:
                AddToSlider(@event.RewardAmount);
                break;
        }
    }

    private int UpdateValue(int currentValue, ReceiveRewardEvent @event)
    {
        var result = currentValue + @event.RewardAmount;
        if(result < 0)
            result = 0;
        GameEventSystem.Send<StatsChangedEvent>(
            new StatsChangedEvent(@event.RewardType, result, @event.KillReward ? 1 : 0));

        return result;
    }

    private float AddToSlider(int rewardAmount)
    {
        var result = rewardAmount * SliderMultiplier;
        PlayerHP += result;
        CheckSLiderValue();
        GameEventSystem.Send<StatsChangedEvent>(new StatsChangedEvent(RewardType.HP, PlayerHP, enemiesKilled: 1));

        return result;
    }

    private void SetHpSlider(PlayerHpEvent @event)
    {
        PlayerHP = @event.CurrentHP;
        CheckSLiderValue();
        GameEventSystem.Send<StatsChangedEvent>(new StatsChangedEvent(RewardType.HP, PlayerHP, enemiesKilled: 0));
    }

    private void CheckSLiderValue()
    {
        if(PlayerHP > MaxSliderValue)
            PlayerHP = MaxSliderValue;
    }

    private void SaveProgress(SaveGameEvent @event)
    {
        var progressData = ServiceLocator.Container.RequestFor<ProgressData>();
        if (@event.ProgressToSave)
        {
            progressData.ArrowsNumber = ArrowNumber;
            progressData.KeysNumber = KeyNumber;
            progressData.EnemiesKilledNumber = EnemyNumber;
            progressData.PlayerHP = PlayerHP;
        }
        else
        {
            LoadProgress(progressData);
        }
    }

    private void LoadProgress(ProgressData progressData)
    {
        ArrowNumber = progressData.ArrowsNumber;
        GameEventSystem.Send<StatsChangedEvent>(
            new StatsChangedEvent(RewardType.Arrow, ArrowNumber));
        KeyNumber = progressData.KeysNumber;
        GameEventSystem.Send<StatsChangedEvent>(
            new StatsChangedEvent(RewardType.Key, KeyNumber));
        EnemyNumber = progressData.EnemiesKilledNumber;
        GameEventSystem.Send<StatsChangedEvent>(
            new StatsChangedEvent(RewardType.Arrow,
                ArrowNumber, enemiesKilled: progressData.EnemiesKilledNumber));
        PlayerHP = progressData.PlayerHP > 0 ? progressData.PlayerHP : MaxSliderValue;
        GameEventSystem.Send<StatsChangedEvent>(
            new StatsChangedEvent(RewardType.HP, PlayerHP));
    }

    public void Dispose()
    {
        GameEventSystem.UnSubscribe<ReceiveRewardEvent>(UpdateStats);
        GameEventSystem.UnSubscribe<SaveGameEvent>(SaveProgress);
        GameEventSystem.UnSubscribe<PlayerHpEvent>(SetHpSlider);
        GC.SuppressFinalize(this);
    }
}