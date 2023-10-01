using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuCanvas : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _audioButton;
    [SerializeField] private Button _gameplayButton;

    [Header("Panels")]
    [SerializeField] private GameObject _settingsMenuPanel;
    [SerializeField] private GameObject _controlsPanel;
    [SerializeField] private AudioPanel _audioPanel;
    [SerializeField] private GamePlayPanel _gameplayPanel;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer _myMixer;

    private SettingsPrefsSaver _prefsSaver;

    public Button ReturnButton => _returnButton;

    private void OnEnable()
    {
        _settingsMenuPanel.SetActive(true);
        _controlsPanel.SetActive(false);
        _audioPanel.Panel.SetActive(false);
        _gameplayPanel.Panel.SetActive(false);

        if (_prefsSaver != null)
            _prefsSaver.AssignValues();
    }

    private void Start()
    {
        _prefsSaver = new SettingsPrefsSaver(_myMixer, _audioPanel.Slider, _audioPanel.SoundSlider, _gameplayPanel.Slider);
        GameEventSystem.Subscribe<SensitivityEvent>(SetMouseSensitivity);

        AssignUI();
    }

    private void AssignUI()
    {
        _audioButton.onClick.AddListener(OpenAudioSubmenu);
        _gameplayButton.onClick.AddListener(OpenGameplaySubmenu);

        _audioPanel.ApplyButton.onClick.AddListener(AudioApply);
        _gameplayPanel.ApplyButton.onClick.AddListener(GameplayApply);

        _audioPanel.DefaultButton.onClick.AddListener(_prefsSaver.SetDefaultAudio);
        _gameplayPanel.DefaultButton.onClick.AddListener(_prefsSaver.SetDefaultGameplay);

        _audioPanel.ReturnButton.onClick.AddListener(_prefsSaver.GetAudioPrefs);
        _gameplayPanel.ReturnButton.onClick.AddListener(_prefsSaver.GetGameplayPrefs);

        _prefsSaver.Init();
    }

    private void SetMouseSensitivity(SensitivityEvent @event) => _gameplayPanel.Value.text = $"{@event.NewValue}";

    private void OpenAudioSubmenu()
    {
        _settingsMenuPanel.SetActive(false);
        _audioPanel.Panel.SetActive(true);
    }

    private void OpenGameplaySubmenu()
    {
        _settingsMenuPanel.SetActive(false);
        _gameplayPanel.Panel.SetActive(true);
    }

    private void AudioApply()
    {
        _prefsSaver.AudioApply();
        _audioPanel.Panel.SetActive(false);
        _settingsMenuPanel.SetActive(true);
    }

    private void GameplayApply()
    {
        _prefsSaver.GameplayApply();
        _gameplayPanel.Panel.SetActive(false);
        _settingsMenuPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        _audioButton.onClick.RemoveListener(OpenAudioSubmenu);
        _gameplayButton.onClick.RemoveListener(OpenGameplaySubmenu);
        _returnButton.onClick.RemoveAllListeners();
        GameEventSystem.UnSubscribe<SensitivityEvent>(SetMouseSensitivity);
        _prefsSaver.Dispose();
    }
}