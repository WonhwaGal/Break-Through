using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public sealed class Bootstrapper : MonoBehaviour, ISceneLoader
{
    public LoadingCurtain Curtain;
    public AudioMixer MainMixer;

    private const string MenuLevel = "MainMenu";
    
    private void Awake() => DontDestroyOnLoad(this);

    private void Start() => LoadNextScene();

    public void LoadNextScene()
    {
        Curtain.Show();
        ServiceLocator.Container.Register<LoadingCurtain>(Curtain);
        SceneManager.LoadSceneAsync(MenuLevel);
        SceneManager.sceneLoaded += OnLoadEvent;
    }

    public void OnLoadEvent(Scene scene, LoadSceneMode mode)
    {
        LoadProgress();
        Curtain.Hide();
        SceneManager.sceneLoaded -= OnLoadEvent;
    }

    private void LoadProgress()
    {
        SetUpAudio();

        if (PlayerPrefs.HasKey(Constants.SaveGameKey))
        {
            string savedJson = PlayerPrefs.GetString(Constants.SaveGameKey);
            ProgressData progressData = JsonUtility.FromJson<ProgressData>(savedJson);
            ServiceLocator.Container.Register<ProgressData>(progressData);
        }
        else
        {
            ServiceLocator.Container.Register<ProgressData>(new ProgressData());
        }
    }

    private void SetUpAudio()
    {
        if (PlayerPrefs.HasKey(Constants.PrefsMusic))
        {
            MainMixer.SetFloat("myMusic", PlayerPrefs.GetInt(Constants.PrefsMusic));
            MainMixer.SetFloat("mySounds", PlayerPrefs.GetInt(Constants.PrefsSound));
        }
        else
        {
            SetDefaultParameters();
        }
    }

    private void SetDefaultParameters()
    {
        MainMixer.SetFloat("myMusic", Constants.DefaultMusic);
        MainMixer.SetFloat("mySounds", Constants.DefaultSound);
        PlayerPrefs.SetInt(Constants.PrefsMusic, (int)Constants.DefaultMusic);
        PlayerPrefs.SetInt(Constants.PrefsSound, (int)Constants.DefaultSound);
        PlayerPrefs.SetInt(Constants.PrefsSensitivity, (int)Constants.DefaultSens);
    }
}