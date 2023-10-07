
public struct SaveGameEvent : IGameEvent
{
    public readonly bool ProgressToSave;

    public SaveGameEvent(bool save) => ProgressToSave = save;
}