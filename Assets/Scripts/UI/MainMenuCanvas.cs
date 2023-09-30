using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvas : BaseSceneUI
{
    [Header("Buttons")]
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _loadLevelButton;

    [Header("Panels")]
    [SerializeField] private GameObject _mainMenuContainer;
    [SerializeField] private GameObject _continueSavedPanel;
    [SerializeField] private GameObject _noSavedGamePanel;
    [SerializeField] private GameObject _rulesPanel;
    
    private void Start()
    {
        SetPanels();
        AssignButtons();
    }

    private void SetPanels()
    {
        _mainMenuContainer.SetActive(true);
        _continueSavedPanel.SetActive(false);
        _noSavedGamePanel.SetActive(false);
        _rulesPanel.SetActive(false);
    }

    private void AssignButtons()
    {
        _exitButton.onClick.AddListener(() => Application.Quit());
        _loadLevelButton.onClick.AddListener(() => SceneManager.LoadScene(1));
        _settingsButton.onClick.AddListener(() => ShowSettingsContainer(_mainMenuContainer));
    }

    private void OnDestroy()
    {
        _exitButton.onClick.RemoveAllListeners();
        _loadLevelButton.onClick.RemoveAllListeners();
        _settingsButton.onClick.RemoveAllListeners();
    }
}