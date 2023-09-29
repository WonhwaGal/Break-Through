using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class EnemyView : MonoBehaviour, IDamagable
{
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private int _damageFromArrow;

    private StateMachine _stateMachine = new();
    private EnemyModel _enemyModel;
    private EnemyAnimator _enemyAnimator;
    private EnemyType _enemyType;

    public NavMeshAgent Agent { get; private set; }
    public EnemyType EnemyType { get => _enemyType; set => _enemyType = value; }
    public EnemyModel Model => _enemyModel;
    public IStateMachine StateMachine => _stateMachine;
    public EnemyAnimator EnemyAnimator => _enemyAnimator;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        _enemyAnimator = new EnemyAnimator(GetComponent<Animator>());
        _enemyModel = new EnemyModel(_shootPoint, _damageFromArrow, _hpSlider);
        _enemyModel.OnMoving += _enemyAnimator.AnimateMovement;
        _enemyModel.OnStartShooting += _enemyAnimator.AnimateShooting;
        _enemyModel.OnDying += _enemyAnimator.AnimateDeath;
    }

    private void OnEnable() => Model.Reset();

    private void Update() => _stateMachine.UpdateStateMachine();

    public void CauseDamage(ArrowType arrowType) => _enemyModel.CauseDamage(arrowType);

    private void OnDisable() => Model.IsDead = false;

    private void OnDestroy()
    {
        _enemyModel.OnMoving -= _enemyAnimator.AnimateMovement;
        _enemyModel.OnStartShooting -= _enemyAnimator.AnimateShooting;
        _enemyModel.OnDying -= _enemyAnimator.AnimateDeath;
        Model.Dispose();
    }
}