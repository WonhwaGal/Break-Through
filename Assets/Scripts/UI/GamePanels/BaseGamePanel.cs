using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BaseGamePanel : MonoBehaviour
{
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        if(_saveButton != null)
        {
            _saveButton.onClick.AddListener(SaveGame);
            if (SceneManager.GetActiveScene().buildIndex == 3)
                _saveButton.interactable = false;
        }
        _exitButton.onClick.AddListener(Quit);
    }

    private void SaveGame()
    {
        GameEventSystem.Send<SaveGameEvent>(new SaveGameEvent(true));
    }

    private void Quit() => Application.Quit();

    protected void BaseOnDestroy()
    {
        if (_saveButton != null)
            _saveButton.onClick.RemoveListener(SaveGame);
        _exitButton.onClick.RemoveListener(Quit);
    }
}