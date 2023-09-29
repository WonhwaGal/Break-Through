
public class ReturnToBaseState : BaseEnemyState
{
    private const float RegularStoppingDist = 0.9f;

    public override void EnterState()
    {
        Owner.Agent.stoppingDistance = RegularStoppingDist;
        Owner.Agent.SetDestination(Owner.Model.GuardPoint);
        Owner.Model.State = typeof(ReturnToBaseState);
        base.EnterState();
    }

    public override void UpdateState()
    {
        if (Owner.Agent.remainingDistance < 1)
        {
            Owner.Agent.isStopped = true;
            Owner.Model.IsIdle = true;

            var owner = Owner;
            StateMachine.ChangeTo<GuardState>(guardState => guardState.Owner = owner);
        }

        if (Owner.Model.Target != null)
        {
            var owner = Owner;
            StateMachine.ChangeTo<ChasePlayerState>(chaseState => chaseState.Owner = owner);
        }
    }
}