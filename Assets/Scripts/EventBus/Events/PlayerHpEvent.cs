public struct PlayerHpEvent : IGameEvent
{
    public readonly int PreviousHP;
    public readonly int CurrentHP;
    public readonly bool IsHurt;

    public PlayerHpEvent(int newHP, int oldHP, bool isHurt)
    {
        PreviousHP = oldHP;
        CurrentHP = newHP;
        IsHurt = isHurt;
    }
}