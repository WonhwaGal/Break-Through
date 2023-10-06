using UnityEngine;

public class BossLoader : MonoBehaviour
{
    [SerializeField] private SpawnScriptableObject _prefabs;
    [SerializeField] private Transform _bossSpawnPoint;

    private void Start()
    {
        EnemyView boss = Instantiate(_prefabs.BossPrefab, _bossSpawnPoint);
        boss.StateMachine.ChangeTo<PatrolState>(patrolState => patrolState.Owner = boss);
        boss.Model.OnReadyToDespawn += WinGame;
    }

    private void WinGame(EnemyView bossView)
    {
        Debug.Log("do when win");
        bossView.Model.OnReadyToDespawn -= WinGame;
    }
}