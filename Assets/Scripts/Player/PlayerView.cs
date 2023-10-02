using UnityEngine;

public class PlayerView : MonoBehaviour, IDamagable
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField, Range(5, 15)] private float _speed = 12;
    [SerializeField] private BowView _bowView;

    private IInputService _input;
    private PlayerMovement _movement;
    private PlayerAnimator _animator;
    private PlayerModel _model;

    public PlayerModel Model => _model;

    void Start()
    {
        _input = ServiceLocator.Container.RequestFor<KeyboardInputService>();
        _movement = new PlayerMovement(_characterController, _speed);
        _animator = new PlayerAnimator(GetComponentInChildren<Animator>());
        _model = new PlayerModel(_animator, _bowView.ShootPoint);
    }

    void Update()
    {
        if (_model.IsDead)
            return;

        _animator.AnimateMovement(_input.KeyAxis);
        
        if (!_model.ShouldStand())
            _movement.Move(_input);
    }

    public void CauseDamage(ArrowType arrowType) => _model.CauseDamage(arrowType);
}