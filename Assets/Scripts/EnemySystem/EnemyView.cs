using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMesh;
    [SerializeField] private Animator _animator;
    [SerializeField] private int _hp;

    private StateMachine _stateMachine = new();
    private EnemyModel _enemyModel = new();
    private EnemyAnimator _enemyAnimator;
    private EnemyType _enemyType;
    private Transform _target;

    public Transform Target
    {
        get => _target;
        set
        {
            _target = value;
            if(_target != null)
            {
                OnSeeingPlayer?.Invoke(true);
                _stateMachine.ChangeTo<ChasePlayerState>(chaseState => chaseState.Owner = this);
            }
            else
            {
                OnSeeingPlayer?.Invoke(false);
                _stateMachine.ChangeTo<ReturnToBaseState>(loseState => loseState.Owner = this);
            }

            OnMoving?.Invoke(true);
        }
    }
    public bool IsIdle
    {
        get => _enemyModel.IsIdle;
        set
        {
            _enemyModel.IsIdle = value;
            if (value)
            {
                OnMoving?.Invoke(false);
                _stateMachine.ChangeTo<GuardState>(guardState => guardState.Owner = this);
            }

        }
    }
    public bool IsShooting { get; set; }
    public EnemyType EnemyType { get => _enemyType; set => _enemyType = value; }
    public EnemyModel EnemyModel { get => _enemyModel; }
    public IStateMachine StateMachine { get => _stateMachine; }

    public event Action<bool> OnSeeingPlayer;
    public event Action<bool> OnMoving;

    private void Awake()
    {
        _stateMachine = new StateMachine();
        _enemyAnimator = new EnemyAnimator(_animator);
        OnMoving += _enemyAnimator.AnimateMovement;
    }

    private void OnEnable() => EnemyModel.SetValues();

    private void Update() => _stateMachine.UpdateStateMachine();

    private void OnDestroy()
    {
        OnMoving -= _enemyAnimator.AnimateMovement;
    }
}
