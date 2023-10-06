
public struct LoadLevelEvent : IGameEvent
{
    public bool LoadFinalLevel;

    public LoadLevelEvent(bool toFinal) => LoadFinalLevel = toFinal;
}