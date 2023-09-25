using System.Threading.Tasks;

public class ChasePlayerState : BaseEnemyState
{
    private const float PlayerStoppingDist = 5;
    private bool _shouldCancel;

    public override void EnterState()
    {
        Owner.Agent.isStopped = false;
        Owner.Agent.stoppingDistance = PlayerStoppingDist;
        TransitionToShoot();
        base.EnterState();
    }

    public override void UpdateState() => ChasePlayer();

    private void ChasePlayer()
    {
        if (Owner.EnemyModel.Target == null)
        {
            var owner = Owner;
            if (Owner.EnemyType == EnemyType.Guard)
                StateMachine.ChangeTo<ReturnToBaseState>(returnState => returnState.Owner = owner);
            else
                StateMachine.ChangeTo<PatrolState>(patrolState => patrolState.Owner = owner);
        }
        else
        {
            CheckProximityToPlayer();
            Owner.Agent.SetDestination(Owner.EnemyModel.Target.position);
        }
    }

    private async void TransitionToShoot()
    {
        await Task.Delay(Owner.EnemyModel.ChaseSpanInMilliSec);
        if (_shouldCancel)
            return;

        var owner = Owner;
        if(Owner.Agent != null && !Owner.EnemyModel.IsDead)
            StateMachine.ChangeTo<ShootState>(shootState => shootState.Owner = owner);
    }

    private void CheckProximityToPlayer()
    {
        bool reachedPlayer = (Owner.EnemyModel.Target.position - Owner.transform.position).magnitude - Owner.Agent.stoppingDistance < 0;

        if (reachedPlayer)
        {
            _shouldCancel = true;
            var owner = Owner;
            StateMachine.ChangeTo<ShootState>(shootState => shootState.Owner = owner);
        }
    }

    public override void ExitState()
    {
        _shouldCancel = true;
        base.ExitState();
    }
}