
public struct LoadLevelEvent : IGameEvent
{
    public readonly bool LoadFinalLevel;
    public readonly bool LoadWithCutscene;
    public LoadLevelEvent(bool toFinal, bool withCutscene)
    {
        LoadFinalLevel = toFinal;
        LoadWithCutscene = withCutscene;
    }
}