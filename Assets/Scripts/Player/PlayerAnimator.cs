using UnityEngine;

public class PlayerAnimator
{
    private Animator _animator;
    private static readonly int s_forwardInput = Animator.StringToHash("Forward");
    private static readonly int s_sideInput = Animator.StringToHash("Side");
    private static readonly int s_aiming = Animator.StringToHash("Aiming");

    public PlayerAnimator(Animator animator) => _animator = animator;

    public Animator Animator => _animator;

    public void AnimateMovement(Vector3 input)
    {
        _animator.SetFloat(s_sideInput, input.x);
        _animator.SetFloat(s_forwardInput, input.z);
    }

    public void AnimateAiming(bool aiming)
    {
        _animator.SetBool(s_aiming, aiming);
    }

    public float GetLengthOfClip(string name)
    {
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i].name == name)
                return clips[i].length;
        }

        return default;
    }
}
