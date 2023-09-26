
public class BaseEnemyState : BaseStateOf<EnemyView>
{
    public override void EnterState()
    {
        Owner.Model.OnDying += GoToDieState;
    }

    public override void ExitState()
    {
        Owner.Model.OnDying -= GoToDieState;
    }

    private void GoToDieState()
    {
        var owner = Owner;
        StateMachine.ChangeTo<DieState>(dieState => dieState.Owner = owner);
    }
}