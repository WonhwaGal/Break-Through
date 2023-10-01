
public struct SensitivityEvent : IGameEvent
{
    public float NewValue;

    public SensitivityEvent(float newValue) => NewValue = newValue;
}
