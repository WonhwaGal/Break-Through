using UnityEngine;
using System.Threading.Tasks;

public class PlayerLoader
{
    private readonly ProgressData _progressData;
    private readonly PlayerView _playerView;
    private const int WaitForEnemySpawn = 300;

    public PlayerLoader(PlayerView playerView, bool continueSavedGame)
    {
        _playerView = playerView;
        _progressData = ServiceLocator.Container.RequestFor<ProgressData>();

        LoadPlayer(continueSavedGame);
    }

    public async void LoadPlayer(bool continueSavedGame)
    {
        if (continueSavedGame && PlayerPrefs.HasKey(Constants.SaveGameKey)
                && _progressData.PlayerPos != Vector3.zero)
        {
            var player = GameObject.Instantiate<PlayerView>(_playerView, _progressData.PlayerPos, Quaternion.identity);
            await Task.Delay(WaitForEnemySpawn);
            player.GetComponentInChildren<WideRangeDetector>().ScanForEnemies();
            GameEventSystem.Send<SaveGameEvent>(new SaveGameEvent(save: false));
        }
        else
        {
            GameObject.Instantiate<PlayerView>(_playerView, Constants.PlayerDefaultSpawn, Quaternion.identity);
            var stats = ServiceLocator.Container.RequestFor<StatisticsCounter>();
            stats.GrantArrows(Constants.StartArrowNumber);
        } 
    }
}