using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerModule : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyView>(out var enemyView))
        {
            enemyView.Target = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EnemyView>(out var enemyView))
        {
            enemyView.Target = null;
        }
    }
}
