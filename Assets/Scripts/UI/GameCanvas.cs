using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : BaseSceneUI
{
    [Header("Game UI")]
    [SerializeField] private Slider _playerHpSlider;
    [SerializeField] private TextMeshProUGUI _hpText;

    [Header("Pause UI")]
    [SerializeField] private GameObject _panel;

    private void Start()
    {
        GameEventSystem.Subscribe<PlayerHpEvent>(SetHpSlider);
        _playerHpSlider.onValueChanged.AddListener(SetHpText);
        _panel.SetActive(false);
    }

    private void SetHpSlider(PlayerHpEvent @event) => _playerHpSlider.value = @event.CurrentHP;
    private void SetHpText(float value) => _hpText.text = value.ToString();


    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe<PlayerHpEvent>(SetHpSlider);
        _playerHpSlider.onValueChanged.RemoveListener(SetHpText);
    }
}