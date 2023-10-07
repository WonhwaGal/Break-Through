
public struct StatsChangedEvent : IGameEvent
{
    public readonly RewardType RewardType;
    public readonly int NewValue;
    public readonly int EnemiesKilled;

    public StatsChangedEvent(RewardType type, int newValue, int enemiesKilled = 0)
    {
        RewardType = type;
        NewValue = newValue;
        EnemiesKilled = enemiesKilled;
    }
}