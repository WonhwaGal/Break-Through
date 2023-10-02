using System;
using UnityEngine;

public class StatisticsCounter : IService, IDisposable
{
    private const int SliderMultiplier = 10;

    public StatisticsCounter()
    {
        GameEventSystem.Subscribe<ReceiveRewardEvent>(UpdateStats);
    }

    public int ArrowNumber { get; private set; }
    public int KeyNumber { get; private set; }
    public int EnemyNumber { get; private set; }


    public void GrantArrows(int number)
    {
        ArrowNumber += number;
        GameEventSystem.Send<StatsChangedEvent>(new StatsChangedEvent(RewardType.Arrow, ArrowNumber, enemyKilled: false));
    }

    private void UpdateStats(ReceiveRewardEvent @event)
    {
        if(@event.RewardAmount <= 0)
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
        bool enemyKilled = @event.RewardAmount > 0 && @event.RewardAmount < Constants.KillRewardMaxValue;
        GameEventSystem.Send<StatsChangedEvent>(new StatsChangedEvent(@event.RewardType, result, enemyKilled));

        return result;
    }

    private float AddToSlider(int rewardAmount)
    {
        var result = rewardAmount * SliderMultiplier;
        GameEventSystem.Send<StatsChangedEvent>(new StatsChangedEvent(RewardType.HP, (int)result, true));

        return result;
    }

    public void Dispose()
    {
        GameEventSystem.Subscribe<ReceiveRewardEvent>(UpdateStats);
        GC.SuppressFinalize(this);
    }
}