using UnityEngine;

public struct PlayMusicEvent : IGameEvent
{
    public readonly AudioClip Clip;
    public readonly bool OnLoop;
    public readonly AudioType AudioType;

    public PlayMusicEvent(AudioClip clip, bool onLoop, AudioType atype)
    {
        Clip = clip;
        AudioType = atype;
        OnLoop = onLoop;
    }
}