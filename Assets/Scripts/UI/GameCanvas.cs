using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : BaseSceneUI
{
    [Header("Game UI")]
    [SerializeField] private Slider _playerHpSlider;
    [SerializeField] private TextMeshProUGUI _hpText;

    [Header("Game State UI")]
    [SerializeField] private GameObject _backgroundPanel;
    [SerializeField] private PausePanel _pausePanel;
    [SerializeField] private GameOverPanel _gameOverPanel;

    private void Start()
    {
        GameEventSystem.Subscribe<PlayerHpEvent>(SetHpSlider);
        GameEventSystem.Subscribe<GameStopEvent>(StopGame);
        _playerHpSlider.onValueChanged.AddListener(SetHpText);
        AssignPausePanelButton(_pausePanel);
        _backgroundPanel.SetActive(false);
        _pausePanel.gameObject.SetActive(false);
        _gameOverPanel.gameObject.SetActive(false);
    }

    private void SetHpSlider(PlayerHpEvent @event) => _playerHpSlider.value = @event.CurrentHP;
    private void SetHpText(float value) => _hpText.text = value.ToString();

    private void StopGame(GameStopEvent @event)
    {
        _backgroundPanel.SetActive(@event.IsPaused || @event.EndOfGame);

        if (!@event.EndOfGame)
            ShowPanel(_pausePanel, @event.IsPaused);
        else
            ShowPanel(_gameOverPanel);
    }


    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe<PlayerHpEvent>(SetHpSlider);
        _playerHpSlider.onValueChanged.RemoveListener(SetHpText);
    }
}