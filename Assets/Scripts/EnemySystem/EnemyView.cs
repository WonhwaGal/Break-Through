using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public sealed class EnemyView : MonoBehaviour, IDamagable, IPausable
{
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private int arrowDamage = Constants.ArrowDamageToEnemy;
    [SerializeField] private Transform _shootPoint;

    private readonly StateMachine _stateMachine = new();
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
        _enemyModel = new EnemyModel(_shootPoint, _hpSlider, arrowDamage, _stateMachine);
        SetUpAnimator();
        GameEventSystem.Subscribe<GameStopEvent>(Pause);
    }

    private void OnEnable() => Model.Reset();

    private void Update()
    {
        if (!Model.IsPaused)
            _stateMachine.UpdateStateMachine();
        else
            ((BaseEnemyState)_stateMachine.CurrentState).AdjustPauseTime(Time.deltaTime);
    }

    public void CauseDamage(ArrowType arrowType) => _enemyModel.CauseDamage(arrowType);

    public void Pause(GameStopEvent @event)
    {
        if (Model.IsDead)
            return;

        Model.Pause(@event);
        if(Agent.isActiveAndEnabled)
            Agent.isStopped = _stateMachine.CurrentState.GetType() == typeof(ShootState) || @event.IsPaused;
    }

    private void SetUpAnimator()
    {
        _enemyAnimator = new EnemyAnimator(GetComponent<Animator>());
        _enemyModel.OnMoving += _enemyAnimator.AnimateMovement;
        _enemyModel.OnStartShooting += _enemyAnimator.AnimateShooting;
        _enemyModel.OnDying += _enemyAnimator.AnimateDeath;
    }

    private void OnDestroy()
    {
        _enemyModel.OnMoving -= _enemyAnimator.AnimateMovement;
        _enemyModel.OnStartShooting -= _enemyAnimator.AnimateShooting;
        _enemyModel.OnDying -= _enemyAnimator.AnimateDeath;
        GameEventSystem.UnSubscribe<GameStopEvent>(Pause);
        _enemyAnimator.Dispose();
        Model.Dispose();
    }
}