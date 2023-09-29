using UnityEngine;

public class ChasePlayerState : BaseEnemyState
{
    private const float PlayerStoppingDist = 6;
    private float _timeToShoot;

    public override void EnterState()
    {
        Owner.Agent.isStopped = false;
        Owner.Agent.stoppingDistance = PlayerStoppingDist;
        _timeToShoot = Time.time + Owner.Model.ChaseTimeSpan;
        Owner.Model.State = typeof(ChasePlayerState);
        base.EnterState();
    }

    public override void UpdateState() => ChasePlayer();

    private void ChasePlayer()
    {
        if(_timeToShoot < Time.time)
        {
            TransitionToShoot();
            return;
        }

        if (Owner.Model.Target == null)
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
            Owner.Agent.SetDestination(Owner.Model.Target.position);
        }
    }

    private void TransitionToShoot()
    {
        var owner = Owner;
        if (Owner.Agent != null && !Owner.Model.IsDead)
            StateMachine.ChangeTo<ShootState>(shootState => shootState.Owner = owner);
    }

    private void CheckProximityToPlayer()
    {
        bool reachedPlayer = (Owner.Model.Target.position - Owner.transform.position).magnitude - Owner.Agent.stoppingDistance < 0;

        if (reachedPlayer)
        {
            var owner = Owner;
            StateMachine.ChangeTo<ShootState>(shootState => shootState.Owner = owner);
        }
    }
}