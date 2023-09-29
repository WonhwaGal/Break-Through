public struct PlayerHpEvent : IGameEvent
{
    public readonly int PreviousHP;
    public readonly int CurrentHP;
    public PlayerHpEvent(int newHP, int oldHP)
    {
        PreviousHP = oldHP;
        CurrentHP = newHP;
    }
}