using UnityEngine;

public struct EnemyKilledEvent : IGameEvent
{
    public readonly RewardType RewardType;
    public readonly int RewardAmount;
    public readonly Vector3 SpawnPosition;

    public EnemyKilledEvent(RewardType type, int rewardAmount, Vector3 pos)
    {
        RewardType = type;
        RewardAmount = rewardAmount;
        SpawnPosition = pos;
    }
}
