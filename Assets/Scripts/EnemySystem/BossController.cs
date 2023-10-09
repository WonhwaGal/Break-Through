using UnityEngine;

public sealed class BossController : MonoBehaviour
{
    [SerializeField] private SpawnScriptableObject _prefabs;
    [SerializeField] private Transform _bossSpawnPoint;
    [SerializeField] private float _stayAfterDeathTime = 1;
    private bool _cancelPlayerWin;

    private void Awake() => GameEventSystem.Subscribe<GameStopEvent>(PlayerDieEvent);

    private void Start()
    {
        EnemyView boss = Instantiate(_prefabs.BossPrefab, _bossSpawnPoint);
        boss.EnemyType = EnemyType.Boss;
        boss.Model.StayAfterDeathTime = _stayAfterDeathTime;
        boss.StateMachine.ChangeTo<PatrolState>(patrolState => patrolState.Owner = boss);
        boss.Model.OnReadyToDespawn += WinGame;
    }

    private void WinGame(EnemyView boss)
    {
        if (!_cancelPlayerWin)
            GameEventSystem.Send<GameStopEvent>(new GameStopEvent(true, isEnded: true, isWin: true));
        boss.Model.OnReadyToDespawn -= WinGame;
    }

    private void PlayerDieEvent(GameStopEvent @event)
    {
        if (@event.EndOfGame && !@event.IsWin)
            _cancelPlayerWin = true;
    }

    private void OnDestroy() => GameEventSystem.UnSubscribe<GameStopEvent>(PlayerDieEvent);
}