using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private SpawnScriptableObject spawnPrefabs;
    [SerializeField] private Transform[] _guardPoints;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private int _minNumberOfEnemies = 1;

    private IPool<EnemyView> _enemyViewPool;

    private void Start()
    {
        _enemyViewPool = new Pool<EnemyView>(spawnPrefabs.EnemyPrefab, transform);
        PlaceGuards();
        PlacePatrols();
    }

    private void Update()
    {
        if (_enemyViewPool.ActiveAgents < _minNumberOfEnemies)
            RandomSpawnEnemy();
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
        EnemyView enemy = _enemyViewPool.Spawn();
        enemy.transform.position = placement;
        enemy.EnemyType = enemyType;
        enemy.Agent.avoidancePriority = Random.Range(0, 51);
        enemy.Model.OnReadyToDespawn += ReturnToPool;
        return enemy;
    }

    private void RandomSpawnEnemy()
    {
        EnemyView enemy = SpawnEnemy(_patrolPoints[Random.Range(0, _patrolPoints.Length)].position, EnemyType.Patrol);
        enemy.StateMachine.ChangeTo<PatrolState>(patrolState => patrolState.Owner = enemy);
    }

    private void ReturnToPool(EnemyView enemy)
    {
        enemy.Model.OnReadyToDespawn -= ReturnToPool;
        _enemyViewPool.Despawn(enemy);
    }
}
