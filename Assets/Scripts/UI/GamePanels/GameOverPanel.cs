using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : BaseGamePanel
{
    [SerializeField] private Button _restartButton;

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
        BaseOnDestroy();
    }
}