using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : BaseGamePanel
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _saveButton;

    private void Start()
    {
        _restartButton.onClick.AddListener(TryAgain);
    }

    private void TryAgain()
    {
        GameEventSystem.Send<RestartGameEvent>(new RestartGameEvent(restart: true));
    }

    private void OnDestroy()
    {
        _restartButton.onClick.RemoveListener(TryAgain);
    }
}