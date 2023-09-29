using System;
using UnityEngine;

public class AgentAnimator : IDisposable
{
    protected Animator _animator;

    public AgentAnimator(Animator animator) => _animator = animator;
    
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

    public bool CheckCurrentClip(string name)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(name))
            return true;

        return false;
    }

    public virtual void Dispose() { }
}