
public class BaseEnemyState : BaseStateOf<EnemyView>
{
    public override void EnterState()
    {
        Owner.EnemyModel.OnDying += GoToDieState;
    }

    public override void ExitState()
    {
        Owner.EnemyModel.OnDying -= GoToDieState;
    }

    private void GoToDieState()
    {
        var owner = Owner;
        StateMachine.ChangeTo<DieState>(dieState => dieState.Owner = owner);
    }
}