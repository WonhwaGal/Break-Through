using System;
using UnityEngine;

public class EnemyAnimator : AgentAnimator
{
    private static readonly int s_moving = Animator.StringToHash("Moving");
    private static readonly int s_shoot = Animator.StringToHash("Shoot");
    private static readonly int s_die = Animator.StringToHash("Die");

    public EnemyAnimator(Animator animator) : base(animator)
    {
        GameEventSystem.Subscribe<GameStopEvent>(PauseAnimation);
    }

    public void AnimateMovement(bool hasTarget, bool shouldMove, Type stateType)
    {
        if (stateType == typeof(GuardState) && !hasTarget)
            return;

        _animator.SetBool(s_moving, shouldMove);
    }

    public void AnimateShooting() => _animator.SetTrigger(s_shoot);

    public void PauseAnimation(GameStopEvent @event) => _animator.speed = @event.IsPaused ? 0 : 1;

    public void AnimateDeath() => _animator.SetTrigger(s_die);
}