using UnityEngine.AI;

public class ChasePlayerState : BaseStateOf<EnemyView>
{
    private NavMeshAgent _navMesh;
    private const float PlayerStoppingDist = 5;

    public override void EnterState() => _navMesh = Owner.GetComponent<NavMeshAgent>();

    public override void UpdateState() => ChasePlayer();

    private void ChasePlayer()
    {
        _navMesh.SetDestination(Owner.Target.position);
        _navMesh.stoppingDistance = PlayerStoppingDist;
    }
}
