using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private SpawnScriptableObject spawnPrefabs;
    [SerializeField] private Transform[] _guardPoints;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private int _MinEnemyNumer = 10;

    private Pool<EnemyView> _enemyViewPool;

    private void Start()
    {
        _enemyViewPool = new Pool<EnemyView>(spawnPrefabs.EnemyPrefab);
        PlaceGuards();
        PlacePatrols();
    }

    private void PlaceGuards()
    {
        for (int i = 0; i < _guardPoints.Length; i++)
        {
            EnemyView enemy = SpawnEnemy(_guardPoints[i].position, EnemyType.Guard);
            enemy.StateMachine.ChangeTo<GuardState>(guardState => guardState.Owner = enemy);
        }
    }

    private void PlacePatrols()
    {
        for (int i = 0; i < _patrolPoints.Length; i++)
        {
            EnemyView enemy = SpawnEnemy(_patrolPoints[i].position, EnemyType.Patrol);
            enemy.StateMachine.ChangeTo<PatrolState>(patrolState => patrolState.Owner = enemy);
        }
    }

    private EnemyView SpawnEnemy(Vector3 placement, EnemyType enemyType)
    {
        EnemyView enemy = _enemyViewPool.Spawn(this.transform);
        enemy.transform.position = placement;
        enemy.EnemyType = enemyType;
        return enemy;
    }


}
