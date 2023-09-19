using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private SpawnScriptableObject spawnPrefabs;
    [SerializeField] private Transform[] _guardPoints;

    private Pool<EnemyView> _enemyViewPool;

    private void Start()
    {
        _enemyViewPool = new Pool<EnemyView>(spawnPrefabs.EnemyPrefab);
        SpawnGuards();
    }

    private void SpawnGuards()
    {
        for (int i = 0; i < _guardPoints.Length; i++)
        {
            EnemyView enemy = _enemyViewPool.Spawn(this.transform);
            enemy.transform.position = _guardPoints[i].position;
            enemy.EnemyType = EnemyType.Guard;
            enemy.StateMachine.ChangeTo<GuardState>(guardState => guardState.Owner = enemy);
        }
    }
}
