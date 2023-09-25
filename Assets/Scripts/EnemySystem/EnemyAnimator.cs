using UnityEngine;

public class EnemyAnimator
{
    private Animator _animator;
    private static readonly int s_moving = Animator.StringToHash("Moving");
    private static readonly int s_shoot = Animator.StringToHash("Shoot");
    private static readonly int s_die = Animator.StringToHash("Die");

    public EnemyAnimator(Animator animator)
    {
        _animator = animator;
    }

    public void AnimateMovement(bool moving)
    {
        _animator.SetBool(s_moving, moving);
    }

    public void AnimateShot()
    {
        _animator.SetTrigger(s_shoot);
    }

    public void AnimateDeath()
    {
        _animator.SetTrigger(s_die);
    }

    public bool CheckCurrentClip(string name)
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName(name))
            return true;

        return false;
    }
}
