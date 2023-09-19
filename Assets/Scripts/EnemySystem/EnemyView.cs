using System;
using UnityEngine;
using UnityEngine.AI;


public interface IState : IDisposable
{
    IStateMachine StateMachine { get; set; }
    void EnterState();
    void ExitState();
    void UpdateState();
}

public interface IStateMachine 
{
    IState CurrentState { get; }
    void ChangeState<T>(Action<T> setUpCallback) where T : IState, new();
    void UpdateStateMachine();
}

public abstract class State<T> : IState
    where T : class
{
    private IStateMachine _stateMachine;
    private T _target;

    public State()
    {
    }
    // 1 ======== > 2 =======> 1
    public IStateMachine StateMachine 
    {
        get => _stateMachine;
        set
        {
            if (_stateMachine != null)
                return;
            _stateMachine = value;
        }
    }

    public T Target
    {
        get => _target;
        set
        {
            if (_target != null)
                return;
            _target = value;
        }
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void UpdateState() { }

    public void Dispose()
    {
        Target = null;
        StateMachine = null;

        GC.SuppressFinalize(this);
    }
}

public class StateMachine : IStateMachine
{
    public IState CurrentState { get; private set; }

    public void ChangeState<T>(Action<T> setUpCallback) where T : IState, new()
    {
        if (CurrentState != null && CurrentState.GetType().Equals(typeof(T)))
            return;
        CurrentState?.ExitState();
        CurrentState?.Dispose();
        //CurrentState = new T();
        var state = Activator.CreateInstance<T>();
        setUpCallback?.Invoke(state);
        state.StateMachine = this;
        CurrentState = state;
        CurrentState.EnterState();
    }

    public void UpdateStateMachine() => CurrentState.UpdateState();
}

public class EnemyView : MonoBehaviour
{
    // state machine
    [SerializeField] private NavMeshAgent _navMesh;
    [SerializeField] private Animator _animator;
    [SerializeField] private int _hp;

    private EnemyModel _enemyModel = new();
    private EnemyAnimator _enemyAnimator;
    private EnemyType _enemyType;
    private Transform _target;
    private StateMachine _stateMachine;
    public Transform Target
    {
        get => _target;
        set
        {
            _target = value;
            if(_target != null)
                OnSeeingPlayer?.Invoke(true);
            else
                OnSeeingPlayer?.Invoke(false);
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
                OnMoving?.Invoke(false);
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

    private void OnDestroy()
    {
        OnMoving -= _enemyAnimator.AnimateMovement;
    }
}
