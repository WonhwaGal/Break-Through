
public struct SensitivityEvent : IGameEvent
{
    public readonly float NewValue;

    public SensitivityEvent(float newValue) => NewValue = newValue;
}
