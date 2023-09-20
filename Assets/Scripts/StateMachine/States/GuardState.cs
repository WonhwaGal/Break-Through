using UnityEngine.AI;

public class GuardState : BaseStateOf<EnemyView>
{
    public override void EnterState()
    {
        Owner.Agent.enabled = true;
        if (Owner.EnemyModel.GuardPoint == UnityEngine.Vector3.zero)
            Owner.EnemyModel.GuardPoint = Owner.transform.position;
    }

    public override void UpdateState()
    {
        if (Owner.Target != null)
        {
            var owner = Owner;
            StateMachine.ChangeTo<ChasePlayerState>(chaseState => chaseState.Owner = owner);
        }
    }
}
