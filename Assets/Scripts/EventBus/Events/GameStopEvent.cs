
public struct GameStopEvent : IGameEvent
{
    public readonly bool EndOfGame;
    public GameStopEvent(bool isEnded) => EndOfGame = isEnded;
}