using UnityEngine;

public sealed class WideRangeDetector : MonoBehaviour
{
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private LayerMask _enemyMask;

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

    public void ScanForEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _collider.radius, _enemyMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].gameObject.SetActive(false);
        }
    }
}
