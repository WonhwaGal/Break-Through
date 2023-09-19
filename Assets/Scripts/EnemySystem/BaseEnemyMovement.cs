using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemyMovement : IDisposable
{
    private EnemyView _enemyView;
    protected NavMeshAgent _navMesh;
    protected bool _targetingPlayer;
    protected bool _isPatrolling;
    protected const float _playerStoppingDist = 5;
    protected const float _regularStoppingDist = 1;

    public EnemyView EnemyView { get => _enemyView; }

    public BaseEnemyMovement(EnemyView view)
    {
        _enemyView = view;
        _navMesh = EnemyView.GetComponent<NavMeshAgent>();
        _navMesh.enabled = true;
        _navMesh.avoidancePriority = UnityEngine.Random.Range(1, 51);
        _enemyView.OnSeeingPlayer += TargetPlayer;
    }
    public void UpdateState()
    {
        _navMesh.isStopped = _enemyView.IsShooting;
        if (!_targetingPlayer && !_isPatrolling && _navMesh.remainingDistance < 1.2f)
            _enemyView.IsIdle = true;

        if (_targetingPlayer)
        {
            ChasePlayer();
        }
    }
    public virtual void Patrol() { }

    public void TargetPlayer(bool seePlayer)
    {
        _targetingPlayer = seePlayer;

        if (!seePlayer)
            LosePlayer();
    }

    public void ChasePlayer()
    {
        _navMesh.SetDestination(_enemyView.Target.position);
        _navMesh.stoppingDistance = _playerStoppingDist;
    }

    protected virtual void LosePlayer() => _navMesh.stoppingDistance = _regularStoppingDist;
    public void Die() => _navMesh.isStopped = true;

    public void Dispose()
    {
        _enemyView.OnSeeingPlayer -= TargetPlayer;
    }
}