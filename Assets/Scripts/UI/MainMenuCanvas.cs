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
    }
    private void OnDestroy()
    {
        _exitButton.onClick.RemoveAllListeners();
    }
}
