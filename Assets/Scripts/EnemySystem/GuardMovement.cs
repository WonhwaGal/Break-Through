using UnityEngine;

public class GuardMovement : BaseEnemyMovement
{
    public Transform _guardPoint;
    public GuardMovement()
    {
    }
    public GuardMovement(EnemyView view, Transform guardPoint) : base(view)
    {
        view.EnemyType = EnemyType.Guard;
        _guardPoint = guardPoint;
    }

    protected override void LosePlayer()
    {
        base.LosePlayer();
        _navMesh.SetDestination(_guardPoint.position);
    }
}

