using UnityEngine;

public class DieState : BaseStateOf<EnemyView>
{
    private float _despawnTime;
    private Vector3 _verticalShift = Constants.RewardSpawnYShift;

    public override void EnterState()
    {
        Owner.Agent.enabled = false;
        Owner.Model.Target = null;
        Owner.GetComponent<Collider>().enabled = false;
        _despawnTime = Time.time + Owner.Model.StayAfterDeathTime;
        var rewardSpawnPos = Owner.transform.position + _verticalShift;
        if(Owner.EnemyType != EnemyType.Boss)
            GameEventSystem.Send<EnemyKilledEvent>(
                new EnemyKilledEvent(Owner.Model.RewardType, Owner.Model.RewardAmount, rewardSpawnPos));
    }

    public override void UpdateState()
    {
        if (_despawnTime < Time.time)
            Owner.Model.InvokeDespawn(Owner);
    }

    public override void ExitState()
    {
        Owner.GetComponent<Collider>().enabled = true;
        base.ExitState();
    }
}