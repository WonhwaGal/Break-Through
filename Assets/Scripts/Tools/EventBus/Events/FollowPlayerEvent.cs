using Cinemachine;

public struct FollowPlayerEvent : IGameEvent
{
    public readonly VirtualCameraManager Manager;

    public FollowPlayerEvent(VirtualCameraManager manager)
    {
        Manager = manager;
    }
}