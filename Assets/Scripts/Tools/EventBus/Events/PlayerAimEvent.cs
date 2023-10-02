public struct PlayerAimEvent : IGameEvent
{
    public readonly bool AimPressed;

    public PlayerAimEvent(bool isAiming) => AimPressed = isAiming;
}
