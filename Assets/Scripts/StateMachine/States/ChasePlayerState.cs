
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
        if(Owner.Target == null)
        {
            Owner.ShootCoroutine(false);
            var owner = Owner;
            StateMachine.ChangeTo<ReturnToBaseState>(returnState => returnState.Owner = owner);
        }
        else
        {
            Owner.Agent.SetDestination(Owner.Target.position);
            Owner.Agent.isStopped = Owner.IsShooting;
        }
    }
}
