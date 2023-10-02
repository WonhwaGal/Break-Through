
public struct RestartGameEvent : IGameEvent
{
    public bool RestartGame;

    public RestartGameEvent(bool restart) => RestartGame = restart;
}