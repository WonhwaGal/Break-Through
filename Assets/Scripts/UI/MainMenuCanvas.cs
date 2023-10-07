using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenuCanvas : BaseSceneUI
{
    [Header("Buttons")]
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _startLevelButton;
    [SerializeField] private Button _loadSavedGameButton;

    [Header("Panels")]
    [SerializeField] private GameObject _mainMenuContainer;
    [SerializeField] private GameObject _continueSavedPanel;
    [SerializeField] private GameObject _noSavedGamePanel;
    [SerializeField] private GameObject _rulesPanel;

    private void Start()
    {
        SetUp();
        AssignButtons();
        GameEventSystem.Send<PlayMusicEvent>(new PlayMusicEvent(
            _soundPrefabs.MenuBackground, onLoop: true, AudioType.BackgroundMusic));
    }

    private void SetUp()
    {
        _mainMenuContainer.SetActive(true);
        _continueSavedPanel.SetActive(false);
        _noSavedGamePanel.SetActive(false);
        _rulesPanel.SetActive(false);
    }

    private void AssignButtons()
    {
        _exitButton.onClick.AddListener(QuitGame);
        _startLevelButton.onClick.AddListener(() => LoadNextScene(false));
        _loadSavedGameButton.onClick.AddListener(() => LoadNextScene(true));
        _settingsButton.onClick.AddListener(() => ShowSettingsContainer(_mainMenuContainer));
    }

    public void LoadNextScene(bool continueSaved)
    {
        _sceneLoader.ContinueSaved = continueSaved;
        if (_sceneLoader.ContinueSaved && !_sceneLoader.HasSavedGame())
        {
            _continueSavedPanel.SetActive(false);
            _noSavedGamePanel.SetActive(true);
            return;
        }

        _sceneLoader.LoadNextScene();
    }

    public void QuitGame()
    {
        GameEventSystem.Send<SaveGameEvent>(new SaveGameEvent(true));
        Application.Quit();
    }

    private void OnDestroy()
    {
        _exitButton.onClick.RemoveAllListeners();
        _startLevelButton.onClick.RemoveAllListeners();
        _loadSavedGameButton.onClick.RemoveAllListeners();
        _settingsButton.onClick.RemoveAllListeners();
        BaseOnDestroy();
    }
}