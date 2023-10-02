using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private PlayerAudio _playerSounds;
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        GameEventSystem.Subscribe<ReceiveRewardEvent>(PlayerShootArrow);
        GameEventSystem.Subscribe<PlayerHpEvent>(PlayerHurtEvent);
    }

    public void PlayerStep() => _audioSource.PlayOneShot(_playerSounds.GroundFootstep);
    public void PlayerShootArrow(ReceiveRewardEvent @event)
    {
        if (@event.RewardType == RewardType.Arrow && @event.RewardAmount < 0)
            _audioSource.PlayOneShot(_playerSounds.ArrowShoot);
    }
    public void PlayerHurtEvent(PlayerHpEvent @event)
    {
        if (@event.CurrentHP > @event.PreviousHP)
            return;

        if (@event.CurrentHP > 0)
        {
            if (!@event.IsHurt)
                _audioSource.PlayOneShot(_playerSounds.PlayerHurt);
        }
        else
        {
            _audioSource.PlayOneShot(_playerSounds.PlayerDie);
        }
    }

    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe<ReceiveRewardEvent>(PlayerShootArrow);
        GameEventSystem.UnSubscribe<PlayerHpEvent>(PlayerHurtEvent);
    }
}