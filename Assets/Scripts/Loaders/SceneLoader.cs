using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneLoader : ISceneLoader
{
    private LoadingCurtain _curtain;
    private SpawnScriptableObject _spawnPrefabs;
    private string _nextScene;
    private bool _continueSaved;

    public const string GameLevel = "GameLevel";
    public const string FinalLevel = "FinalScene";

    public bool ContinueSaved
    {
        get => _continueSaved;
        set
        {
            _continueSaved = value;
            PlayerPrefs.SetInt(Constants.ContinueSaved, value ? 1 : 0);
        }
    }
    public bool LoadFinalLevel { get; set; }

    public SceneLoader(SpawnScriptableObject scriptableObject)
    {
        _spawnPrefabs = scriptableObject;
        ContinueSaved = PlayerPrefs.GetInt(Constants.ContinueSaved) == 1;
        _curtain = ServiceLocator.Container.RequestFor<LoadingCurtain>();
    }

    public void LoadNextScene()
    {
        _curtain.Show();

        if (LoadFinalLevel)
            _nextScene = FinalLevel;
        else
            _nextScene = GameLevel;

        SceneManager.LoadSceneAsync(_nextScene);
        SceneManager.sceneLoaded += OnLoadEvent;
    }

    public void OnLoadEvent(Scene scene, LoadSceneMode mode)
    {
        _curtain.Hide();
        PrepareGameServices(_spawnPrefabs);
        new PlayerLoader(_spawnPrefabs.PlayerPrefab, ContinueSaved, LoadFinalLevel);
        SceneManager.sceneLoaded -= OnLoadEvent;
    }

    private void PrepareGameServices(SpawnScriptableObject spawnPrefabs)
    {
        ServiceLocator.Container.Register<StatisticsCounter>(new StatisticsCounter());
        ServiceLocator.Container.Register<KeyboardInputService>(new KeyboardInputService());
        ServiceLocator.Container.Register<ArrowController>(new ArrowController(spawnPrefabs));
        ServiceLocator.Container.Register<Pointer>(
            new Pointer(spawnPrefabs.PointerPrefab, ServiceLocator.Container.RequestFor<KeyboardInputService>()));
    }

    public bool HasSavedGame()
    {
        if (!PlayerPrefs.HasKey(Constants.SaveGameKey))
            return false;

        return true;
    }
}