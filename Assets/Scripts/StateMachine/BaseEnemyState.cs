
using UnityEngine;

public class BaseEnemyState : BaseStateOf<EnemyView>
{
    protected const float RegularStoppingDist = 0.9f;
    protected const float PlayerStoppingDist = 6;

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

    public virtual void AdjustPauseTime(float deltaTime) { }
}