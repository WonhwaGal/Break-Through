
public struct StatsChangedEvent : IGameEvent
{
    public RewardType RewardType;
    public int NewValue;
    public bool EnemyKilled;

    public StatsChangedEvent(RewardType type, int newValue, bool enemyKilled)
    {
        RewardType = type;
        NewValue = newValue;
        EnemyKilled = enemyKilled;
    }
}