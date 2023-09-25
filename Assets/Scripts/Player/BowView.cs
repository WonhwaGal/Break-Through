using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BowView : MonoBehaviour
{
    private Animator _animator;

    private void OnEnable() => _animator = GetComponent<Animator>();

    public void TightenBow(bool aiming) => _animator.SetBool("Aim", aiming);
}
