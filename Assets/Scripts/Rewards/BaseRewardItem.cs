using UnityEngine;

public class BaseRewardItem : MonoBehaviour, IPausable
{
    [SerializeField] private bool KillReward;
    protected bool _isPaused;

    public RewardType RewardType { get; set; }
    public int RewardAmount { get; set; }

    private void Start()
    {
        GameEventSystem.Subscribe<GameStopEvent>(Pause);
    }

    public void Pause(GameStopEvent @event) => _isPaused = @event.IsPaused;

    public void SendReceiveAwardEvent()
    {
        GameEventSystem.Send<ReceiveRewardEvent>(new ReceiveRewardEvent(RewardType, RewardAmount, KillReward));
    }

    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe<GameStopEvent>(Pause);
    }
}