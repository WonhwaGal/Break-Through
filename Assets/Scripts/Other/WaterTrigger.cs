using UnityEngine;

public class WaterTrigger : MonoBehaviour, IPausable
{
    [SerializeField] private GameAudioScriptable gameAudioScriptable;
    [SerializeField] private AudioSource _audioSource;

    private Vector3 _triggerCenter;
    private float _startDistance;
    private float _distance;
    private float _generalSoundVolume;
    private bool _isPaused;

    private const float TerrainXCenter = -200;
    
    public Transform PlayerT { get; private set; }

    private void Start()
    {
        GameEventSystem.Subscribe<GameStopEvent>(Pause);
    }
    private void Update()
    {
        if (PlayerT == null || _isPaused)
            return;

        _distance = (_triggerCenter - PlayerT.position).magnitude;
        var distReference = (_startDistance - _distance) / _startDistance;
        var middleValue = (distReference + _generalSoundVolume) / 2;
        _audioSource.volume = middleValue;
    }

    public void ReceivePlayer(Transform playerT, Vector3 triggerCenter)
    {
        PlayerT = playerT;
        _triggerCenter = triggerCenter;
        _startDistance = (_triggerCenter - PlayerT.position).magnitude;

        SetVolume();
        PlaySound();
    }

    private void SetVolume()
    {
        float soundOutputValue
    = Constants.MaxAudioValue - PlayerPrefs.GetInt(Constants.PrefsSound);
        float soundRange = (Constants.MaxAudioValue - Constants.MinAudioValue);
        _generalSoundVolume = soundOutputValue / soundRange;
    }

    private void PlaySound()
    {
        _audioSource.volume = 0;
        _startDistance = (_triggerCenter - PlayerT.position).magnitude;
        if (_triggerCenter.x > TerrainXCenter)
            _audioSource.clip = gameAudioScriptable.RiverSound;
        else
            _audioSource.clip = gameAudioScriptable.WaterfallSound;
        _audioSource.Play();
    }

    public void PlayerToNull()
    {
        PlayerT = null;
        _audioSource.Stop();
    }

    public void Pause(GameStopEvent @event)
    {
        _isPaused = @event.IsPaused || @event.EndOfGame;
        if (_isPaused)
        {
            _audioSource.Stop();
        }
        else
        {
            SetVolume();
            _audioSource.Play();
        }
    }

    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe<GameStopEvent>(Pause);
    }
}