using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class EnemyView : MonoBehaviour
{
    [SerializeField] private int _hp;

    private StateMachine _stateMachine = new();
    private EnemyModel _enemyModel = new();
    private EnemyAnimator _enemyAnimator;
    private EnemyType _enemyType;
    private Transform _target;
    private bool _isShooting;
    private WaitForSeconds _shootInterval = new WaitForSeconds(5);

    public Transform Target
    {
        get => _target;
        set
        {
            _target = value;
            if (_target != null)
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
    public NavMeshAgent Agent { get; private set; }
    public bool IsShooting
    {
        get => _isShooting;
        set
        {
            _isShooting = value;
            if (value)
                OnTakingAShot?.Invoke();
        }
    }
    public EnemyType EnemyType { get => _enemyType; set => _enemyType = value; }
    public EnemyModel EnemyModel { get => _enemyModel; }
    public IStateMachine StateMachine { get => _stateMachine; }

    public event Action<bool> OnSeeingPlayer;
    public event Action<bool> OnMoving;
    public event Action OnTakingAShot;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        _enemyAnimator = new EnemyAnimator(GetComponent<Animator>());
        OnMoving += _enemyAnimator.AnimateMovement;
        OnTakingAShot += _enemyAnimator.Shoot;
    }

    private void OnEnable() => EnemyModel.SetValues();

    private void Update() => _stateMachine.UpdateStateMachine();

    public void ShootCoroutine(bool play)
    {
        if (play)
            StartCoroutine(Shoot());
        else
            StopCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (_target != null)
        {
            yield return _shootInterval;
            IsShooting = _target == null ? false : true;
        }
    }

    public void ShootToFalse() => IsShooting = false;

    private void OnDestroy()
    {
        OnMoving -= _enemyAnimator.AnimateMovement;
        OnTakingAShot -= _enemyAnimator.Shoot;
    }
}
