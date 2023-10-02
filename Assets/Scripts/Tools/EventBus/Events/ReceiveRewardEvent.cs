
public struct ReceiveRewardEvent : IGameEvent
{
    public RewardType RewardType;
    public int RewardAmount;

    public ReceiveRewardEvent(RewardType type, int amount)
    {
        RewardType = type;
        RewardAmount = amount;
    }
}
