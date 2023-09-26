using UnityEngine;

public class WideRangeDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyView>(out var enemyView))
        {
            enemyView.Model.Target = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EnemyView>(out var enemyView))
        {
            enemyView.Model.Target = null;
        }
    }
}
