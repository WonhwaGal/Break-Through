using UnityEngine;

public class DieState : BaseStateOf<EnemyView>
{
    private float _despawnTime;

    public override void EnterState()
    {
        Owner.Agent.isStopped = true;
        Owner.Model.Target = null;
        Owner.Model.State = typeof(DieState);
        Owner.GetComponent<Collider>().enabled = false;
        _despawnTime = Time.time + Owner.Model.StayAfterDeathTime;
    }

    public override void UpdateState()
    {
        if (_despawnTime < Time.time)
        {
            Owner.Model.InvokeDespawn(Owner);
            Dispose();
        }
    }

    public override void ExitState()
    {
        Owner.GetComponent<Collider>().enabled = true;
        base.ExitState();
    }
}