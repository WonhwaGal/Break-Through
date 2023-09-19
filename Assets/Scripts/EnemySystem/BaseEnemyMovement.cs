using UnityEngine.AI;

public abstract class BaseEnemyMovement : BaseStateOf<EnemyView>
{
    private EnemyView _enemyView;
    protected NavMeshAgent _navMesh;
    protected bool _targetingPlayer;
    protected bool _isPatrolling;
    protected const float PlayerStoppingDist = 5;
    protected const float RegularStoppingDist = 1;

    public EnemyView EnemyView { get => _enemyView; }
    public BaseEnemyMovement()
    {

    }
    public BaseEnemyMovement(EnemyView view)
    {
        _enemyView = view;
        _navMesh = EnemyView.GetComponent<NavMeshAgent>();
        _navMesh.enabled = true;
        _navMesh.avoidancePriority = UnityEngine.Random.Range(1, 51);
        _enemyView.OnSeeingPlayer += TargetPlayer;
    }
    public void Update()
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
        _navMesh.stoppingDistance = PlayerStoppingDist;
    }

    protected virtual void LosePlayer() => _navMesh.stoppingDistance = RegularStoppingDist;
    public void Die()
    {
        _navMesh.isStopped = true;
        _enemyView.OnSeeingPlayer -= TargetPlayer;
    }
}