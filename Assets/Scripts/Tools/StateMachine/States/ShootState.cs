using UnityEngine;

public sealed class ShootState : BaseEnemyState
{
    private const float AnimationTransitionTime = 1.0f;
    private const float RotationTime = 0.4f;
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
        Owner.Model.UpdateShooter();
        RotateTowardsPlayer();

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

    private void RotateTowardsPlayer()
    {
        if (Owner.Model.Target == null)
            return;

        var newDir = Owner.Model.Target.position - Owner.transform.position;
        var targetRotation = Quaternion.LookRotation(newDir);
        Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation, targetRotation, RotationTime);
    }

    public override void AdjustPauseTime(float deltaTime)
    {
        _targetTime += deltaTime;
        Owner.Model.AdjustShooter(deltaTime);
    }
}