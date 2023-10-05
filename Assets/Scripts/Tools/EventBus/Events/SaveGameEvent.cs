
public struct SaveGameEvent : IGameEvent
{
    public bool ProgressToSave;

    public SaveGameEvent(bool save) => ProgressToSave = save;
}