using UnityEngine;

public class ShootState : BaseEnemyState
{
    private const float AnimationTransitionTime = 1.0f;
    private float _targetTime;

    public override void EnterState()
    {
        Owner.Model.IsShooting = true;
        Owner.Agent.isStopped = true;
        _targetTime = AnimationTransitionTime + Time.time;
        base.EnterState();
    }

    public override void UpdateState()
    {
        if (_targetTime > Time.time)
            return;

        if (!Owner.EnemyAnimator.CheckCurrentClip("EnemyShoot"))
        {
            Owner.Model.IsShooting = false;
            Owner.Agent.isStopped = false;
            SetNextState();
        }
    }

    public void SetNextState()
    {
        if (Owner.Model.Target != null)
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
}