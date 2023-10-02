using UnityEngine;

public struct EnemyKilledEvent : IGameEvent
{
    public RewardType RewardType;
    public int RewardAmount;
    public Vector3 SpawnPosition;

    public EnemyKilledEvent(RewardType type, int rewardAmount, Vector3 pos)
    {
        RewardType = type;
        RewardAmount = rewardAmount;
        SpawnPosition = pos;
    }
}
