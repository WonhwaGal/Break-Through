
public struct ReceiveRewardEvent : IGameEvent
{
    public RewardType RewardType;
    public int RewardAmount;
    public bool KillReward;

    public ReceiveRewardEvent(RewardType type, int amount, bool killReward)
    {
        RewardType = type;
        RewardAmount = amount;
        KillReward = killReward;
    }
}
