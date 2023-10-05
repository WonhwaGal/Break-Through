using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : BaseSceneUI, ISceneLoader
{
    [SerializeField] private SpawnScriptableObject _spawnPrefabs;

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

    private const string GameLevel = "GameLevel";
    private bool _continueSaved;

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
        _startLevelButton.onClick.AddListener(() => LoadNextScene(continueSaved: false));
        _loadSavedGameButton.onClick.AddListener(() => LoadNextScene(continueSaved: true));
        _settingsButton.onClick.AddListener(() => ShowSettingsContainer(_mainMenuContainer));
    }

    public void LoadNextScene(bool continueSaved)
    {
        if (continueSaved && !HasSavedGame())
            return;

        _curtain.Show();
        _continueSaved = continueSaved;
        SceneManager.LoadSceneAsync(GameLevel);
        SceneManager.sceneLoaded += OnLoadEvent;
    }

    public void OnLoadEvent(Scene scene, LoadSceneMode mode)
    {
        _curtain.Hide();
        PrepareGameServices(_spawnPrefabs);
        new PlayerLoader(_spawnPrefabs.PlayerPrefab, _continueSaved);
        SceneManager.sceneLoaded -= OnLoadEvent;
    }

    private bool HasSavedGame()
    {
        if (!PlayerPrefs.HasKey(Constants.SaveGameKey))
        {
            _continueSavedPanel.SetActive(false);
            _noSavedGamePanel.SetActive(true);
            return false;
        }
        return true;
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