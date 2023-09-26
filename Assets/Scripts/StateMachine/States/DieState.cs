using UnityEngine;

public class DieState : BaseStateOf<EnemyView>
{
    private float _despawnTime;

    public override void EnterState()
    {
        Owner.Agent.isStopped = true;
        Owner.Model.Target = null;
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
}