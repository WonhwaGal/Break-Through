
public class GuardState : BaseEnemyState
{
    public override void EnterState()
    {
        Owner.Agent.enabled = true;
        if (Owner.Model.GuardPoint == UnityEngine.Vector3.zero)
            Owner.Model.GuardPoint = Owner.transform.position;
        base.EnterState();
    }

    public override void UpdateState()
    {
        if (Owner.Model.Target != null)
        {
            var owner = Owner;
            StateMachine.ChangeTo<ChasePlayerState>(chaseState => chaseState.Owner = owner);
        }
    }
}