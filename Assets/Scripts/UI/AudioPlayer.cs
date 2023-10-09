using UnityEngine;

public sealed class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundSource;
    private AudioSource _currentSource;

    private void Awake()
    {
        GameEventSystem.Subscribe<PlayMusicEvent>(PlayAudio);
        DontDestroyOnLoad(gameObject);
    }

    private void PlayAudio(PlayMusicEvent @event)
    {
        if(@event.AudioType == AudioType.BackgroundMusic)
            _currentSource = _musicSource;
        else
            _currentSource = _soundSource;

        _currentSource.loop = @event.OnLoop;
        _currentSource.clip = @event.Clip;
        _currentSource.Play();
    }

    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe<PlayMusicEvent>(PlayAudio);
    }
}