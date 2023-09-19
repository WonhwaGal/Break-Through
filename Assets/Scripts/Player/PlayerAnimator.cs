using System;
using UnityEngine;

public class PlayerAnimator
{
    private Animator _animator;
    private static readonly int _forwardInput = Animator.StringToHash("Forward");
    private static readonly int _sideInput = Animator.StringToHash("Side");
    private static readonly int _aiming = Animator.StringToHash("Aiming");

    public PlayerAnimator(Animator animator) => _animator = animator;

    public void AnimateMovement(Vector3 input)
    {
        _animator.SetFloat(_sideInput, input.x);
        _animator.SetFloat(_forwardInput, input.z);
    }

    public void AnimateAiming(bool aiming) => _animator.SetBool(_aiming, aiming);
}
