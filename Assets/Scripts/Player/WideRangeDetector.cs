using UnityEngine;

public class WideRangeDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyView>(out var enemyView))
        {
            enemyView.EnemyModel.Target = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EnemyView>(out var enemyView))
        {
            enemyView.EnemyModel.Target = null;
        }
    }
}
