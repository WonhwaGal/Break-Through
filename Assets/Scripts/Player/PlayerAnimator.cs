using UnityEngine;

public class PlayerAnimator: AgentAnimator
{
    private static readonly int s_forwardInput = Animator.StringToHash("Forward");
    private static readonly int s_sideInput = Animator.StringToHash("Side");
    private static readonly int s_aiming = Animator.StringToHash("Aiming");
    private static readonly int s_hurt = Animator.StringToHash("Hurt");
    private static readonly int s_die = Animator.StringToHash("Die");

    public PlayerAnimator(Animator animator) : base(animator) 
    {
        GameEventSystem.Subscribe<PlayerAimEvent>(AnimateAiming);
    }

    public Animator Animator => _animator;

    public void AnimateMovement(Vector3 input)
    {
        _animator.SetFloat(s_sideInput, input.x);
        _animator.SetFloat(s_forwardInput, input.z);
    }

    public void AnimateAiming(PlayerAimEvent @event) => _animator.SetBool(s_aiming, @event.AimPressed);

    public void AnimateDamage() => _animator.SetTrigger(s_hurt);
    public void AnimateDeath() => _animator.SetTrigger(s_die);

    public override void Dispose()
    {
        GameEventSystem.UnSubscribe<PlayerAimEvent>(AnimateAiming);
    }
}