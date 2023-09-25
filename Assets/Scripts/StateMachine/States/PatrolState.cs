using UnityEngine;
using UnityEngine.AI;

public class PatrolState : BaseEnemyState
{
    private const float WanderDistance = 15; 
    private const float Threshold = 5;
    private float _maxDistance;
    private Vector3 _lastPosition;

    public override void EnterState()
    {
        _maxDistance = Owner.Agent.height * 2;
        _lastPosition = Owner.transform.position;
        Owner.Agent.enabled = true;
        Owner.EnemyModel.Target = null;
        SetRandomDestination();
        base.EnterState();
    }

    public override void UpdateState()
    {
        if (Owner.Agent.remainingDistance < Threshold)
            SetRandomDestination();

        if (Owner.EnemyModel.Target != null)
        {
            var owner = Owner;
            StateMachine.ChangeTo<ChasePlayerState>(chaseState => chaseState.Owner = owner);
        }
    }

    private void SetRandomDestination()
    {
        if(FindRandomPoint(Owner.transform.position, _maxDistance, out Vector3 result))
            Owner.Agent.SetDestination(result);
        else
            Owner.Agent.SetDestination(_lastPosition);
    }

    private bool FindRandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 newPos = center + WanderDistance * new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(newPos, out hit, range, NavMesh.AllAreas))
        {
            result = hit.position;
            _lastPosition = Owner.transform.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}