
public struct StatsChangedEvent : IGameEvent
{
    public RewardType RewardType;
    public int NewValue;
    public int EnemiesKilled;

    public StatsChangedEvent(RewardType type, int newValue, int enemiesKilled = 0)
    {
        RewardType = type;
        NewValue = newValue;
        EnemiesKilled = enemiesKilled;
    }
}