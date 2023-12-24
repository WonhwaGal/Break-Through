using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using System.Threading.Tasks;

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
        GameEventSystem.Subscribe<GameStopEvent>(PauseCamera);
        GameEventSystem.Subscribe<PlayerHpEvent>(ReactToDeath);
        GameEventSystem.Subscribe<LoadLevelEvent>(PlayWithCutScene);
    }

    private async void Start()
    {
        _followEvent = new FollowPlayerEvent(this);
        await Task.Delay(200);
        GameEventSystem.Send(_followEvent);
    }

    public void PlayWithCutScene(LoadLevelEvent @event)
    {
        if (@event.LoadWithCutscene)
            _director.Play();
    }

    public void SetUpCameras(Transform targetT, float multiplier)
    {
        _virtualCamera.Follow = targetT;
        _virtualCamera.LookAt = targetT;
        _deathCamera.Follow = targetT;
        _deathCamera.LookAt = targetT;
        _pov.m_HorizontalAxis.m_MaxSpeed *= multiplier;
        _pov.m_HorizontalAxis.Value = targetT.rotation.eulerAngles.y;
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
        GameEventSystem.UnSubscribe<LoadLevelEvent>(PlayWithCutScene);
    }
}