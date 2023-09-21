using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _settingsMenuPanel;
    [SerializeField] private GameObject _controlsPanel;
    [SerializeField] private GameObject _soundsPanel;
    [SerializeField] private GameObject _gameplayPanel;
    [SerializeField] private Button _returnButton;

    public Button ReturnButton { get => _returnButton; set => _returnButton = value; }

    private void OnEnable()
    {
        _settingsMenuPanel.SetActive(true);
        _controlsPanel.SetActive(false);
        _soundsPanel.SetActive(false);
        _gameplayPanel.SetActive(false);
    }

    
}
