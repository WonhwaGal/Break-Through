using UnityEngine.AI;

public class ReturnToBaseState : BaseStateOf<EnemyView>
{
    private NavMeshAgent _navMesh;
    private const float RegularStoppingDist = 0.9f;

    public override void EnterState()
    {
        _navMesh = Owner.GetComponent<NavMeshAgent>();
        _navMesh.stoppingDistance = RegularStoppingDist;
        _navMesh.SetDestination(Owner.EnemyModel.GuardPoint);
    }

    public override void UpdateState()
    {
        if (_navMesh.remainingDistance < 1)
        {
            _navMesh.isStopped = true;
            Owner.IsIdle = true;
        }
    }
}
