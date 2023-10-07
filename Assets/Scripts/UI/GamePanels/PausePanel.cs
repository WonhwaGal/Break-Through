using UnityEngine;
using UnityEngine.UI;

public sealed class PausePanel : BaseGamePanel
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _settingsButton;

    public Button SettingsButton => _settingsButton;

    private void Start() => _resumeButton.onClick.AddListener(ResumeGame);

    private void ResumeGame() =>
        GameEventSystem.Send<GameStopEvent>(new GameStopEvent(isPaused: false, isEnded: false, false));

    private void OnDestroy()
    {
        _resumeButton.onClick.RemoveListener(ResumeGame);
        BaseOnDestroy();
    }
}