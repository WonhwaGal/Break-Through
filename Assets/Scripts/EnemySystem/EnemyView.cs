using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMesh;
    [SerializeField] private Animator _animator;
    [SerializeField] private int _hp;

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

    public event Action<bool> OnSeeingPlayer;
    public event Action<bool> OnMoving;

    private void Awake()
    {
        _enemyAnimator = new EnemyAnimator(_animator);
        OnMoving += _enemyAnimator.AnimateMovement;
    }

    private void OnEnable() => EnemyModel.SetValues();

    private void OnDestroy()
    {
        OnMoving -= _enemyAnimator.AnimateMovement;
    }
}
