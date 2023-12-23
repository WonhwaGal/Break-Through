using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class VirtualCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private CinemachineVirtualCamera _deathCamera;
    [SerializeField] private PlayableDirector _director;
    private FollowPlayerEvent _followEvent;
    private CinemachinePOV _pov;
    private float _horSpeed;

    private void Awake()
    {
        _pov = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        _horSpeed = _pov.m_HorizontalAxis.m_MaxSpeed;
    }

    private void OnEnable()
    {
        _followEvent = new FollowPlayerEvent(this);
        GameEventSystem.Send(_followEvent);
        GameEventSystem.Subscribe<GameStopEvent>(PauseCamera);
        GameEventSystem.Subscribe<PlayerHpEvent>(ReactToDeath);
    }

    public void SetUpCameras(Transform transform, float multiplier)
    {
        _deathCamera.gameObject.SetActive(true);
        _virtualCamera.Follow = transform;
        _virtualCamera.LookAt = transform;
        _deathCamera.Follow = transform;
        _deathCamera.LookAt = transform;
        _pov.m_HorizontalAxis.m_MaxSpeed *= multiplier;
    }

    private void PauseCamera(GameStopEvent @event)
    {
        _pov.m_HorizontalAxis.m_MaxSpeed = @event.IsPaused ? 0 : _horSpeed;
        _pov.m_VerticalAxis.m_MaxSpeed = @event.IsPaused ? 0 : _horSpeed;
        if (@event.IsPaused)
            _director.Pause();
        else
            _director.Resume();
    }

    private void ReactToDeath(PlayerHpEvent @event) 
        => _virtualCamera.gameObject.SetActive(@event.CurrentHP > 0);

    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe<GameStopEvent>(PauseCamera);
        GameEventSystem.UnSubscribe<PlayerHpEvent>(ReactToDeath);
    }
}