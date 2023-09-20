
public class ReturnToBaseState : BaseStateOf<EnemyView>
{
    private const float RegularStoppingDist = 0.9f;

    public override void EnterState()
    {
        Owner.Agent.stoppingDistance = RegularStoppingDist;
        Owner.Agent.SetDestination(Owner.EnemyModel.GuardPoint);
    }

    public override void UpdateState()
    {
        Owner.Agent.isStopped = Owner.IsShooting;

        if (Owner.Agent.remainingDistance < 1)
        {
            Owner.Agent.isStopped = true;
            Owner.IsIdle = true;
            var owner = Owner;
            StateMachine.ChangeTo<GuardState>(guardState => guardState.Owner = owner);
        }

        if (Owner.Target != null)
        {
            var owner = Owner;
            StateMachine.ChangeTo<ChasePlayerState>(chaseState => chaseState.Owner = owner);
        }
    }

    public override void ExitState()
    {
        Owner.Agent.SetDestination(Owner.transform.position);
    }
}
