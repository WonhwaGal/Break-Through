using UnityEngine;

public sealed class ChasePlayerState : BaseEnemyState
{
    private float _timeToShoot;

    public override void EnterState()
    {
        Owner.Agent.isStopped = false;
        if(Owner.EnemyType == EnemyType.Boss)
        {
            Owner.Agent.stoppingDistance = RegularStoppingDist;
            _timeToShoot = Time.time + BossChaseSpan;
        }
        else
        {
            Owner.Agent.stoppingDistance = WarriorStoppingDist;
            _timeToShoot = Time.time + ChaseTimeSpan;
        }
        base.EnterState();
    }

    public override void UpdateState() => ChasePlayer();

    private void ChasePlayer()
    {
        if (_timeToShoot < Time.time)
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
            Owner.Agent.SetDestination(Owner.Model.Target.position);
            CheckProximityToPlayer();
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
        bool reachedPlayer = (Owner.Agent.destination - Owner.transform.position).magnitude - Owner.Agent.stoppingDistance < 0;

        if (reachedPlayer)
        {
            var owner = Owner;
            StateMachine.ChangeTo<ShootState>(shootState => shootState.Owner = owner);
        }
    }

    public override void AdjustPauseTime(float deltaTime) => _timeToShoot += deltaTime;
}