
using UnityEngine;

public class ChasePlayerState : BaseStateOf<EnemyView>
{
    private const float PlayerStoppingDist = 5;

    public override void EnterState()
    {
        Owner.Agent.stoppingDistance = PlayerStoppingDist;
        Owner.ShootCoroutine(true);
    }

    public override void UpdateState() => ChasePlayer();

    private void ChasePlayer()
    {
        if(Owner.Target != null)
        {
            Owner.Agent.SetDestination(Owner.Target.position);
            Owner.Agent.isStopped = Owner.IsShooting;
        }
        else
        {
            Owner.ShootCoroutine(false);
            var owner = Owner;
            if (Owner.EnemyType == EnemyType.Guard)
                StateMachine.ChangeTo<ReturnToBaseState>(returnState => returnState.Owner = owner);
            else
                StateMachine.ChangeTo<PatrolState>(patrolState => patrolState.Owner = owner);
        }
    }
}
