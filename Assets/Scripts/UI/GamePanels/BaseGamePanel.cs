using UnityEngine;
using UnityEngine.UI;

public class BaseGamePanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;

    public Button SettingsButton { get => _settingsButton; }

    private void Start()
    {
        _exitButton.onClick.AddListener(Quit);
    }

    private void Quit() => Application.Quit();

    private void OnDestroy()
    {
        _exitButton.onClick.RemoveListener(Quit);
    }
}