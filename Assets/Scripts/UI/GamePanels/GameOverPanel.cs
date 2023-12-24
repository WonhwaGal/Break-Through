using UnityEngine;
using UnityEngine.UI;

public sealed class GameOverPanel : BaseGamePanel
{
    [SerializeField] private Button _restartButton;

    private void Start()
    {
        _restartButton.onClick.AddListener(TryAgain);
    }

    private void TryAgain()
    {
        GameEventSystem.Send(new LoadLevelEvent(toFinal: false, withCutscene: false));
    }

    private void OnDestroy()
    {
        _restartButton.onClick.RemoveListener(TryAgain);
        BaseOnDestroy();
    }
}