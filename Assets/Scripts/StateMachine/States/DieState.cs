

public class DieState : BaseStateOf<EnemyView>
{
    public override void EnterState()
    {
        Owner.Agent.isStopped = true;
        Owner.EnemyModel.Target = null;
    }
}
