using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public sealed class GameCanvas : BaseSceneUI
{
    [Header("Game UI")]
    [SerializeField] private Transform _rewardsT;

    [Header("Game State UI")]
    [SerializeField] private GameObject _backgroundPanel;
    [SerializeField] private PausePanel _pausePanel;
    [SerializeField] private GameOverPanel _gameOverPanel;

    private Button _pauseSettingsButton;
    private RewardController _rewardController;
    private AudioClip _background;

    private void Start()
    {
        SetUp();
        GameEventSystem.Subscribe<GameStopEvent>(StopGame);
        GameEventSystem.Subscribe<LoadLevelEvent>(ReceiveLoadEvent);
        GameEventSystem.Send<PlayMusicEvent>(new PlayMusicEvent(
            _background, onLoop: true, AudioType.BackgroundMusic));
        _rewardController = new RewardController(_prefabs, _rewardsT);
    }

    private void SetUp()
    {
        AssignPausePanelButton(_pausePanel);
        _backgroundPanel.SetActive(false);
        _pausePanel.gameObject.SetActive(false);
        _gameOverPanel.gameObject.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex != 3)
            _background = _soundPrefabs.NatureBackground;
        else
            _background = _soundPrefabs.FinalBackground;
    }

    public void AssignPausePanelButton(PausePanel pausePanel)
    {
        _pauseSettingsButton = pausePanel.SettingsButton;
        _pauseSettingsButton.onClick.AddListener(() => ShowSettingsContainer(pausePanel.gameObject));
    }

    private void StopGame(GameStopEvent @event)
    {
        if (!@event.EndOfGame)
            ShowPanel(_pausePanel, @event.IsPaused);
        else if (!@event.IsWin)
            ShowPanel(_gameOverPanel);
        else
            LoadWinPanel();

        var isOnStop = @event.IsPaused || @event.EndOfGame;
        _backgroundPanel.SetActive(isOnStop);
        AudioClip clip = isOnStop ? _soundPrefabs.MenuBackground : _background;
        GameEventSystem.Send<PlayMusicEvent>(new PlayMusicEvent(clip, onLoop: true, AudioType.BackgroundMusic));
    }

    private void ReceiveLoadEvent(LoadLevelEvent @event)
    {
        _sceneLoader.LoadFinalLevel = @event.LoadFinalLevel;
        _sceneLoader.LoadNextScene();
    }

    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe<GameStopEvent>(StopGame);
        GameEventSystem.UnSubscribe<LoadLevelEvent>(ReceiveLoadEvent);
        _pauseSettingsButton.onClick.RemoveAllListeners();
        _rewardController.Dispose();
        BaseOnDestroy();
    }
}