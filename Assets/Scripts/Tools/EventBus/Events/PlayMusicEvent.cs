using UnityEngine;

public struct PlayMusicEvent : IGameEvent
{
    public AudioClip Clip;
    public bool OnLoop;
    public AudioType AudioType;

    public PlayMusicEvent(AudioClip clip, bool onLoop, AudioType atype)
    {
        Clip = clip;
        AudioType = atype;
        OnLoop = onLoop;
    }
}