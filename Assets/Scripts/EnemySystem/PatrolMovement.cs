public class PatrolMovement : BaseEnemyMovement
{
    public PatrolMovement(EnemyView view) : base(view)
    {
        view.EnemyType = EnemyType.Patrol;
    }

    public override void Patrol() { }

    protected override void LosePlayer()
    {
        base.LosePlayer();
    }
}

