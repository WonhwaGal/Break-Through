using System.Threading.Tasks;

public class ShootState : BaseEnemyState
{
    private const int DefaultTransitionInMilliSec = 300;
    private bool _checkAnimationFinish = false;

    public override void EnterState()
    {
        Owner.EnemyModel.IsShooting = true;
        Owner.Agent.isStopped = true;
        WaitForAnimationTransition();
        base.EnterState();
    }

    public override void UpdateState()
    {
        if (!_checkAnimationFinish)
            return;

        if (!Owner.EnemyAnimator.CheckCurrentClip("EnemyShoot"))
        {
            Owner.EnemyModel.IsShooting = false;
            Owner.Agent.isStopped = false;
            SetNextState();
        }
    }

    public void SetNextState()
    {
        if (Owner.EnemyModel.Target != null)
            ActWhileSeeingPlayer();
        else
            ActAfterLosingPlayer();
    }

    private void ActWhileSeeingPlayer()
    {
        var owner = Owner;
        StateMachine.ChangeTo<ChasePlayerState>(chaseState => chaseState.Owner = owner);
    }

    private void ActAfterLosingPlayer()
    {
        var owner = Owner;
        if (Owner.EnemyType == EnemyType.Guard)
            StateMachine.ChangeTo<ReturnToBaseState>(returnState => returnState.Owner = owner);
        else
            StateMachine.ChangeTo<PatrolState>(patrolState => patrolState.Owner = owner);
    }

    private async void WaitForAnimationTransition()
    {
        await Task.Delay(DefaultTransitionInMilliSec);
        _checkAnimationFinish = true;
    }
}