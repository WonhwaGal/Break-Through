using UnityEngine;
using System.Threading.Tasks;

public class PlayerLoader
{
    private readonly ProgressData _progressData;
    private readonly PlayerView _playerView;
    private const int WaitForEnemySpawn = 300;

    public PlayerLoader(PlayerView playerView, bool continueSavedGame, bool toFinalLevel)
    {
        _playerView = playerView;
        _progressData = ServiceLocator.Container.RequestFor<ProgressData>();

        LoadPlayer(continueSavedGame, toFinalLevel);
    }

    public void LoadPlayer(bool continueSavedGame, bool toFinalLevel)
    {
        var stats = ServiceLocator.Container.RequestFor<StatisticsCounter>();
        if (toFinalLevel)
            SpawnInFinal();
        else if (continueSavedGame && PlayerPrefs.HasKey(Constants.SaveGameKey)
                && _progressData.PlayerPos != Vector3.zero)
            SpawnInSavedSpot();
        else
            DefaultSpawn(stats);
    }

    private void SpawnInFinal()
    {
        GameObject.Instantiate<PlayerView>(_playerView, Constants.PlayerFinalSpawn, Quaternion.identity);
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

    private void DefaultSpawn(StatisticsCounter stats)
    {
        GameObject.Instantiate<PlayerView>(_playerView, Constants.PlayerDefaultSpawn, Quaternion.identity);
        GameEventSystem.Send<ReceiveRewardEvent>(new ReceiveRewardEvent(RewardType.Arrow, Constants.StartArrowNumber, false));
    }
}