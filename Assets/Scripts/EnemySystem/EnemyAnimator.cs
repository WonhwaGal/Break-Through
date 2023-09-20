using UnityEngine;

public class EnemyAnimator
{
    private Animator _animator;
    private static readonly int _moving = Animator.StringToHash("Moving");
    private static readonly int _shoot = Animator.StringToHash("Shoot");

    public EnemyAnimator(Animator animator) => _animator = animator;

    public void AnimateMovement(bool moving)
    {
        _animator.SetBool(_moving, moving);
    }

    public void Shoot()
    {
        _animator.SetTrigger(_shoot);
    }
}
