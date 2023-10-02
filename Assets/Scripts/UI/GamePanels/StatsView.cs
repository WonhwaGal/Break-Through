using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsView : MonoBehaviour
{
    [Header("HP:")]
    [SerializeField] private Slider _playerHpSlider;
    [SerializeField] private TextMeshProUGUI _hpText;

    [Header("Stats:")]
    [SerializeField] private TextMeshProUGUI _arrowsText;
    [SerializeField] private TextMeshProUGUI _keyText;
    [SerializeField] private TextMeshProUGUI _enemiesKilledText;
    private int _enemiesKilled = 0;
    private const int MaxSliderValue = 100;

    private void Awake()
    {
        GameEventSystem.Subscribe<StatsChangedEvent>(UpdateView);
        GameEventSystem.Subscribe<PlayerHpEvent>(SetHpSlider);
        _playerHpSlider.onValueChanged.AddListener(SetHpText);
    }

    private void UpdateView(StatsChangedEvent @event)
    {
        AddEnemies(@event.EnemyKilled);

        switch (@event.RewardType)
        {
            case RewardType.Arrow:
                UpdateSlot(_arrowsText, @event.NewValue);
                break;
            case RewardType.Key:
                UpdateSlot(_keyText, @event.NewValue);
                break;
            default:
                _playerHpSlider.value += @event.NewValue;
                if (_playerHpSlider.value > MaxSliderValue)
                    _playerHpSlider.value = MaxSliderValue;
                break;
        }
    }

    private void AddEnemies(bool enemyKilled)
    {
        if (!enemyKilled)
            return; 

        _enemiesKilled++;
        _enemiesKilledText.text = _enemiesKilled.ToString();
        EnableSprite(_enemiesKilledText, _enemiesKilled > 0);
    }

    private void UpdateSlot(TextMeshProUGUI text, int newValue)
    {
        text.text = newValue.ToString();
        EnableSprite(text, newValue > 0);
    }

    private void EnableSprite(TextMeshProUGUI text, bool shouldEnable)
    {
        Transform spriteT = text.transform.GetChild(0);
        spriteT.gameObject.SetActive(shouldEnable);
    }

    private void SetHpSlider(PlayerHpEvent @event) => _playerHpSlider.value = @event.CurrentHP;
    private void SetHpText(float value) => _hpText.text = value.ToString();

    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe<StatsChangedEvent>(UpdateView);
        GameEventSystem.UnSubscribe<PlayerHpEvent>(SetHpSlider);
        _playerHpSlider.onValueChanged.RemoveListener(SetHpText);
    }
}