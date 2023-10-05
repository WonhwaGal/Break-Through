using UnityEngine;

public class BaseSceneUI : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] protected UIScriptableObject _prefabs;
    [SerializeField] protected GameAudioScriptable _soundPrefabs;
    [SerializeField] private GameObject _settingsSpawn;

    private SettingsMenu _settingsCanvas;
    protected LoadingCurtain _curtain;
    protected ProgressSaver _progressSaver;

    private void Awake()
    {
        _curtain = ServiceLocator.Container.RequestFor<LoadingCurtain>();
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

    protected void PrepareGameServices(SpawnScriptableObject spawnPrefabs)
    {
        ServiceLocator.Container.Register<StatisticsCounter>(new StatisticsCounter());
        ServiceLocator.Container.Register<KeyboardInputService>(new KeyboardInputService());
        ServiceLocator.Container.Register<ArrowController>(new ArrowController(spawnPrefabs));
        ServiceLocator.Container.Register<Pointer>(
            new Pointer(spawnPrefabs.PointerPrefab, ServiceLocator.Container.RequestFor<KeyboardInputService>()));
    }

    protected void BaseOnDestroy()
    {
        if(_settingsCanvas != null)
            _settingsCanvas.ReturnButton.onClick.RemoveAllListeners();
        GameEventSystem.UnSubscribe<SaveGameEvent>(SaveProgress);
    }
}