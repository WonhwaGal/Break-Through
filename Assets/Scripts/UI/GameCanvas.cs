using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : BaseSceneUI
{
    [Header("Game UI")]
    [SerializeField] private Transform _rewardsT;

    [Header("Game State UI")]
    [SerializeField] private GameObject _backgroundPanel;
    [SerializeField] private PausePanel _pausePanel;
    [SerializeField] private GameOverPanel _gameOverPanel;

    private Button _pauseSettingsButton;
    private RewardController _rewardController;

    private void Start()
    {
        SetPanels();
        GameEventSystem.Subscribe<GameStopEvent>(StopGame);
        GameEventSystem.Send<PlayMusicEvent>(new PlayMusicEvent(
            _soundPrefabs.NatureBackground, onLoop: true, AudioType.BackgroundMusic));
        _rewardController = new RewardController(_prefabs, _rewardsT);
    }

    private void StopGame(GameStopEvent @event)
    {
        if (!@event.EndOfGame)
            ShowPanel(_pausePanel, @event.IsPaused);
        else
            ShowPanel(_gameOverPanel);

        _backgroundPanel.SetActive(@event.IsPaused || @event.EndOfGame);
        AudioClip clip = @event.IsPaused ? _soundPrefabs.MenuBackground : _soundPrefabs.NatureBackground;
        GameEventSystem.Send<PlayMusicEvent>(new PlayMusicEvent(clip, onLoop: true, AudioType.BackgroundMusic));
    }

    private void SetPanels()
    {
        AssignPausePanelButton(_pausePanel);
        _backgroundPanel.SetActive(false);
        _pausePanel.gameObject.SetActive(false);
        _gameOverPanel.gameObject.SetActive(false);
    }

    public void AssignPausePanelButton(PausePanel pausePanel)
    {
        _pauseSettingsButton = pausePanel.SettingsButton;
        _pauseSettingsButton.onClick.AddListener(() => ShowSettingsContainer(pausePanel.gameObject));
    }

    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe<GameStopEvent>(StopGame);
        _pauseSettingsButton.onClick.RemoveAllListeners();
        _rewardController.Dispose();
        BaseOnDestroy();
    }
}