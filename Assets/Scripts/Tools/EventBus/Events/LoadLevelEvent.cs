
public struct LoadLevelEvent : IGameEvent
{
    public readonly bool LoadFinalLevel;

    public LoadLevelEvent(bool toFinal) => LoadFinalLevel = toFinal;
}