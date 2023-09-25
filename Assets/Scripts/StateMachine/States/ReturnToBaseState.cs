
public class ReturnToBaseState : BaseEnemyState
{
    private const float RegularStoppingDist = 0.9f;

    public override void EnterState()
    {
        Owner.Agent.stoppingDistance = RegularStoppingDist;
        Owner.Agent.SetDestination(Owner.EnemyModel.GuardPoint);
        base.EnterState();
    }

    public override void UpdateState()
    {
        if (Owner.Agent.remainingDistance < 1)
        {
            Owner.Agent.isStopped = true;
            Owner.EnemyModel.IsIdle = true;

            var owner = Owner;
            StateMachine.ChangeTo<GuardState>(guardState => guardState.Owner = owner);
        }

        if (Owner.EnemyModel.Target != null)
        {
            var owner = Owner;
            StateMachine.ChangeTo<ChasePlayerState>(chaseState => chaseState.Owner = owner);
        }
    }
}