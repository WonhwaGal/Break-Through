using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton; // + 
    [SerializeField] private Button _loadLevelButton;

    [Header("Panels")]
    [SerializeField] private GameObject _mainMenuContainer;
    [SerializeField] private GameObject _confirmNewPanel;
    [SerializeField] private GameObject _continueSavedPanel;
    [SerializeField] private GameObject _noSavedGamePanel;
    [SerializeField] private GameObject _rulesPanel;

    [Header("Other")]
    [SerializeField] private SpawnScriptableObject _prefabs;

    private void Start()
    {
        SetPanels();
        AssignButtons();
    }

    private void SetPanels()
    {
        _mainMenuContainer.SetActive(true);
        _confirmNewPanel.SetActive(false);
        _continueSavedPanel.SetActive(false);
        _noSavedGamePanel.SetActive(false);
        _rulesPanel.SetActive(false);
    }
    private void AssignButtons()
    {
        _exitButton.onClick.AddListener(() => Application.Quit());
        _loadLevelButton.onClick.AddListener(() => SceneManager.LoadScene(1));
        _settingsButton.onClick.AddListener(LoadSettingsContainer);
    }

    private void LoadSettingsContainer()
    {
        _mainMenuContainer.SetActive(false);
        SettingsMenuCanvas settings = Instantiate<SettingsMenuCanvas>(_prefabs.SettingsMenuPrefab);
        settings.transform.SetParent(this.transform);
        settings.transform.SetAsLastSibling();
        settings.ReturnButton.onClick.AddListener(() => _mainMenuContainer.SetActive(true));
    }
    private void OnDestroy()
    {
        _exitButton.onClick.RemoveAllListeners();
        _loadLevelButton.onClick.RemoveAllListeners();
        _settingsButton.onClick.RemoveAllListeners();
    }
}
