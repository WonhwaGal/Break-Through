using UnityEngine;

public class PlayerAnimator: AgentAnimator
{
    private static readonly int s_forwardInput = Animator.StringToHash("Forward");
    private static readonly int s_sideInput = Animator.StringToHash("Side");
    private static readonly int s_aiming = Animator.StringToHash("Aiming");
    private static readonly int s_hurt = Animator.StringToHash("Hurt");
    private static readonly int s_die = Animator.StringToHash("Die");

    private readonly StatisticsCounter _stats;

    public PlayerAnimator(Animator animator) : base(animator) 
    {
        GameEventSystem.Subscribe<PlayerAimEvent>(AnimateAiming);
        GameEventSystem.Subscribe<GameStopEvent>(PauseAnimation);
        _stats = ServiceLocator.Container.RequestFor<StatisticsCounter>();
    }

    public Animator Animator => _animator;

    public void AnimateMovement(Vector3 input)
    {
        _animator.SetFloat(s_sideInput, input.x);
        _animator.SetFloat(s_forwardInput, input.z);
    }

    public void AnimateAiming(PlayerAimEvent @event)
    {
        if (_stats.ArrowNumber > 0)
            _animator.SetBool(s_aiming, @event.AimPressed);
    }

    public void AnimateDamage() => _animator.SetTrigger(s_hurt);
    public void AnimateDeath() => _animator.SetTrigger(s_die);
    private void PauseAnimation(GameStopEvent @event) => _animator.speed = @event.IsPaused ? 0 : 1;

    public override void Dispose()
    {
        GameEventSystem.UnSubscribe<PlayerAimEvent>(AnimateAiming);
        GameEventSystem.UnSubscribe<GameStopEvent>(PauseAnimation);
    }
}