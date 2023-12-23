using UnityEngine;
using System.Threading.Tasks;

public sealed class PlayerLoader
{
    private readonly ProgressData _progressData;
    private readonly PlayerView _playerView;
    private readonly Transform _defaultSpawn;
    private readonly Transform _finalSpawn;
    private const int WaitForEnemySpawn = 300;

    public PlayerLoader(SpawnScriptableObject spawnPrefabs, bool continueSavedGame, bool toFinalLevel)
    {
        _playerView = spawnPrefabs.PlayerPrefab;
        _defaultSpawn = spawnPrefabs.PlayerDefaultSpawn.transform;
        _finalSpawn = spawnPrefabs.PlayerFinalSpawn.transform;
        _progressData = ServiceLocator.Container.RequestFor<ProgressData>();

        LoadPlayer(continueSavedGame, toFinalLevel);
    }

    public void LoadPlayer(bool continueSavedGame, bool toFinalLevel)
    {
        if (toFinalLevel)
            SpawnInFinal();
        else if (continueSavedGame && PlayerPrefs.HasKey(Constants.SaveGameKey)
                && _progressData.PlayerPos != Vector3.zero)
            SpawnInSavedSpot();
        else
            DefaultSpawn();
    }

    private void SpawnInFinal()
    {
        GameObject.Instantiate<PlayerView>(_playerView, _finalSpawn.position, _finalSpawn.rotation);
        GameEventSystem.Send<ReceiveRewardEvent>(
            new ReceiveRewardEvent(RewardType.Arrow, PlayerPrefs.GetInt(Constants.CurrentArrowNumber), false));
    }

    private async void SpawnInSavedSpot()
    {
        var player = GameObject.Instantiate<PlayerView>(_playerView, _progressData.PlayerPos, Quaternion.identity);
        await Task.Delay(WaitForEnemySpawn);
        player.GetComponentInChildren<WideRangeDetector>().ScanForEnemies();
        GameEventSystem.Send<SaveGameEvent>(new SaveGameEvent(save: false));
    }

    private void DefaultSpawn()
    {
        GameObject.Instantiate<PlayerView>(_playerView, _defaultSpawn.position, _defaultSpawn.rotation);
        GameEventSystem.Send<ReceiveRewardEvent>(new ReceiveRewardEvent(RewardType.Arrow, Constants.StartArrowNumber, false));
    }
}