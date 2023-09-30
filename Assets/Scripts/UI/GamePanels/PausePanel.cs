using UnityEngine;
using UnityEngine.UI;

public class PausePanel : BaseGamePanel
{
    [SerializeField] private Button _resumeButton;

    private void Start()
    {
        _resumeButton.onClick.AddListener(ResumeGame);
    }

    private void ResumeGame()
    {
        GameEventSystem.Send<GameStopEvent>(new GameStopEvent(isEnded: false, isPaused: false));
    }

    private void OnDestroy()
    {
        _resumeButton.onClick.RemoveListener(ResumeGame);
    }
}