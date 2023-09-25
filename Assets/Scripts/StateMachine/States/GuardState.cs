
public class GuardState : BaseEnemyState
{
    public override void EnterState()
    {
        Owner.Agent.enabled = true;
        if (Owner.EnemyModel.GuardPoint == UnityEngine.Vector3.zero)
            Owner.EnemyModel.GuardPoint = Owner.transform.position;
        base.EnterState();
    }

    public override void UpdateState()
    {
        if (Owner.EnemyModel.Target != null)
        {
            var owner = Owner;
            StateMachine.ChangeTo<ChasePlayerState>(chaseState => chaseState.Owner = owner);
        }
    }
}