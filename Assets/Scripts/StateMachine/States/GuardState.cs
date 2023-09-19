using UnityEngine.AI;

public class GuardState : BaseStateOf<EnemyView>
{
    public override void EnterState()
    {
        Owner.GetComponent<NavMeshAgent>().enabled = true;
        if(Owner.EnemyModel.GuardPoint == UnityEngine.Vector3.zero)
            Owner.EnemyModel.GuardPoint = Owner.transform.position;
    }
}
