
public struct ReceiveRewardEvent : IGameEvent
{
    public readonly RewardType RewardType;
    public readonly int RewardAmount;
    public readonly bool KillReward;

    public ReceiveRewardEvent(RewardType type, int amount, bool killReward)
    {
        RewardType = type;
        RewardAmount = amount;
        KillReward = killReward;
    }
}
