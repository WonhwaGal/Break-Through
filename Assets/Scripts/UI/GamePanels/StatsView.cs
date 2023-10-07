using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public sealed class StatsView : MonoBehaviour
{
    [Header("HP:")]
    [SerializeField] private Slider _playerHpSlider;
    [SerializeField] private TextMeshProUGUI _hpText;

    [Header("No Arrow Panel")]
    [SerializeField] private GameObject _noArrowPanel;

    [Header("Stats:")]
    [SerializeField] private TextMeshProUGUI _arrowsText;
    [SerializeField] private TextMeshProUGUI _keyText;
    [SerializeField] private TextMeshProUGUI _enemiesKilledText;
    private int _enemiesKilled = 0;

    private void Awake()
    {
        GameEventSystem.Subscribe<StatsChangedEvent>(UpdateView);
        GameEventSystem.Subscribe<PlayerAimEvent>(ShowNoArrowPanel);
        _playerHpSlider.onValueChanged.AddListener(SetHpText);
        _noArrowPanel.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            _keyText.gameObject.SetActive(false);
            _enemiesKilledText.gameObject.SetActive(false);
        }
    }

    private void UpdateView(StatsChangedEvent @event)
    {
        EnemyKilled(@event.EnemiesKilled);

        switch (@event.RewardType)
        {
            case RewardType.Arrow:
                UpdateSlot(_arrowsText, @event.NewValue);
                break;
            case RewardType.Key:
                UpdateSlot(_keyText, @event.NewValue);
                break;
            default:
                _playerHpSlider.value = @event.NewValue;
                break;
        }
    }

    private void EnemyKilled(int enemiesKilled)
    {
        _enemiesKilled += enemiesKilled;
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

    private void SetHpText(float value) => _hpText.text = value.ToString();

    private void ShowNoArrowPanel(PlayerAimEvent @event)
    {
        if(PlayerPrefs.GetInt(Constants.CurrentArrowNumber) == 0)
            _noArrowPanel.SetActive(@event.AimPressed);
    }

    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe<StatsChangedEvent>(UpdateView);
        GameEventSystem.UnSubscribe<PlayerAimEvent>(ShowNoArrowPanel);
        _playerHpSlider.onValueChanged.RemoveListener(SetHpText);
    }
}