using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private SpawnScriptableObject spawnPrefabs;
    [SerializeField] private Transform[] _guardPoints;

    private List<BaseEnemyMovement> enemyList = new();
    private Pool<EnemyView> _enemyViewPool;
    private bool _objectsSpawned = false;

    private void Start()
    {
        _enemyViewPool = new Pool<EnemyView>(spawnPrefabs.EnemyPrefab);
        SpawnGuards();
    }

    private void Update()
    {
        if (!_objectsSpawned)
            return;

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].UpdateState();
        }
    }

    private void SpawnGuards()
    {
        for (int i = 0; i < _guardPoints.Length; i++)
        {
            EnemyView enemy = _enemyViewPool.Spawn(this.transform);
            enemy.transform.position = _guardPoints[i].position;
            enemyList.Add(new GuardMovement(enemy, _guardPoints[i]));
        }
        _objectsSpawned = true;
    }
}
