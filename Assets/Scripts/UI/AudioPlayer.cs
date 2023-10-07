using UnityEngine;

public sealed class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundSource;

    private void Awake()
    {
        GameEventSystem.Subscribe<PlayMusicEvent>(PlayAudio);
        DontDestroyOnLoad(gameObject);
    }

    private void PlayAudio(PlayMusicEvent @event)
    {
        _musicSource.loop = @event.OnLoop;
        _musicSource.clip = @event.Clip;

        if(@event.AudioType == AudioType.BackgroundMusic)
            _musicSource.Play();
        else
            _soundSource.Play();
    }

    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe<PlayMusicEvent>(PlayAudio);
    }
}