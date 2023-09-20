using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField, Range(0, 10)] private float _speed;

    private KeyboardInputService _input;
    private PlayerMovement _movement;
    private PlayerAnimator _animator;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  //temporarily
        _input = new KeyboardInputService();
        _movement = new PlayerMovement(_characterController, transform, _speed);
        _animator = new PlayerAnimator(GetComponentInChildren<Animator>());
        _input.OnPressingAim += _animator.AnimateAiming;
    }

    void Update()
    {
        _animator.AnimateMovement(_input.KeyAxis);
        _movement.Move(_input);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        _input.OnPressingAim -= _animator.AnimateAiming;
    }
}