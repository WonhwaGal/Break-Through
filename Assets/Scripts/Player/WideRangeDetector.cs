using UnityEngine;

public class WideRangeDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyView>(out var enemyView))
        {
            enemyView.Model.Target = this.transform;
        }
        else if(other.transform.parent.TryGetComponent<WaterTrigger>(out var water))
        {
            water.ReceivePlayer(transform, other.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EnemyView>(out var enemyView))
        {
            enemyView.Model.Target = null;
        }
        else if (other.TryGetComponent<WaterTrigger>(out var water))
        {
            water.PlayerToNull();
        }
    }
}
