using UnityEngine;

[RequireComponent(typeof(Animator))]
public sealed class BowView : MonoBehaviour
{
    public Transform ShootPoint;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        GameEventSystem.Subscribe<PlayerAimEvent>(TightenBow);
    }

    public void TightenBow(PlayerAimEvent @event) => _animator.SetBool("Aim", @event.AimPressed);

    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe<PlayerAimEvent>(TightenBow);
    }
}
