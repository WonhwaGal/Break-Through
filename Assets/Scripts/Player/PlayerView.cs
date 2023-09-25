using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField, Range(0, 10)] private float _speed;
    [SerializeField] private BowView _bowView;

    private IInputService _input;
    private PlayerMovement _movement;
    private PlayerAnimator _animator;
    private PlayerModel _model;

    public PlayerModel Model => _model;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  //temporarily

        _input = new KeyboardInputService();
        _movement = new PlayerMovement(_characterController, transform, _speed);
        _animator = new PlayerAnimator(GetComponentInChildren<Animator>());
        _model = new PlayerModel(_animator);
        _input.OnPressingAim += _animator.AnimateAiming;
        _input.OnPressingAim += _bowView.TightenBow;
        _input.OnPressingAim += _model.StartAiming;
    }

    void Update()
    {
        _animator.AnimateMovement(_input.KeyAxis);
        
        if (!_model.IsShooting)
            _movement.Move(_input);
    }

    private void OnDestroy()
    {
        _input.OnPressingAim -= _animator.AnimateAiming;
        _input.OnPressingAim -= _bowView.TightenBow;
        _input.OnPressingAim -= _model.StartAiming;
    }
}