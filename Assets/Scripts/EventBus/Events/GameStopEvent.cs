
public struct GameStopEvent : IGameEvent
{
    public readonly bool EndOfGame;
    public readonly bool IsPaused;
    public GameStopEvent(bool isEnded, bool isPaused)
    {
        EndOfGame = isEnded;
        IsPaused = isPaused;
    }
}