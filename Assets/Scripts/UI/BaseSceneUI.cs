using UnityEngine;

public class BaseSceneUI : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] protected UIScriptableObject _prefabs;
    [SerializeField] protected GameAudioScriptable _soundPrefabs;
    [SerializeField] protected SpawnScriptableObject _spawnPrefabs;
    [SerializeField] private GameObject _settingsSpawn;

    private SettingsMenu _settingsCanvas;
    private WinPanel _winPanel;
    protected LoadingCurtain _curtain;
    protected ProgressSaver _progressSaver;
    protected SceneLoader _sceneLoader;

    private void Awake()
    {
        _sceneLoader = new SceneLoader(_spawnPrefabs);
        GameEventSystem.Subscribe<SaveGameEvent>(SaveProgress);
    }

    protected void ShowSettingsContainer(GameObject invokingPanel)
    {
        invokingPanel.SetActive(false);

        if (_settingsCanvas == null)
        {
            _settingsCanvas = Instantiate<SettingsMenu>(_prefabs.SettingsMenu, _settingsSpawn.transform);
            _settingsCanvas.transform.SetAsLastSibling();
            _settingsCanvas.ReturnButton.onClick.AddListener(() => HideSettings(invokingPanel, _settingsCanvas.gameObject));
            return;
        }

        _settingsCanvas.gameObject.SetActive(true);
    }

    protected void LoadWinPanel()
    {
        GameEventSystem.Send<PlayMusicEvent>(new PlayMusicEvent(_soundPrefabs.GongSound, false, AudioType.Sound));
        _winPanel = Instantiate<WinPanel>(_prefabs.WinPanel, transform);
        _winPanel.transform.SetAsLastSibling();
        _winPanel.gameObject.SetActive(true);
    }

    public void ShowPanel(BaseGamePanel panel, bool shouldShow = true)
    {
        if (panel != null)
            panel.gameObject.SetActive(shouldShow);
    }

    private void HideSettings(GameObject invokingPanel, GameObject settingsPanel)
    {
        invokingPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    private void SaveProgress(SaveGameEvent @event)
    {
        _progressSaver = new ProgressSaver();
        _progressSaver.SaveProgressData();
    }

    protected void BaseOnDestroy()
    {
        if(_settingsCanvas != null)
            _settingsCanvas.ReturnButton.onClick.RemoveAllListeners();
        GameEventSystem.UnSubscribe<SaveGameEvent>(SaveProgress);
    }
}