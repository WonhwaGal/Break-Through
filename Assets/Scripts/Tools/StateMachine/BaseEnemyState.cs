
public class BaseEnemyState : BaseStateOf<EnemyView>
{
    protected const float RegularStoppingDist = 0.9f;
    protected const float WarriorStoppingDist = 6.0f;
    protected const float ChaseTimeSpan = 3.0f;
    protected const float BossChaseSpan = 1.5f;
    protected const float StayAfterDeathTime = 5.0f;
    protected const float BossAfterDeathTime = 2.0f;

    public override void EnterState() => Owner.Model.OnDying += GoToDieState;

    public override void ExitState() => Owner.Model.OnDying -= GoToDieState;

    private void GoToDieState()
    {
        var owner = Owner;
        StateMachine.ChangeTo<DieState>(dieState => dieState.Owner = owner);
    }

    public virtual void AdjustPauseTime(float deltaTime) { }
}