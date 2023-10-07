
public struct GameStopEvent : IGameEvent
{
    public readonly bool EndOfGame;
    public readonly bool IsPaused;
    public readonly bool IsWin;

    public GameStopEvent(bool isPaused, bool isEnded, bool isWin)
    {
        EndOfGame = isEnded;
        IsPaused = isPaused;
        IsWin = isWin;
    }
}