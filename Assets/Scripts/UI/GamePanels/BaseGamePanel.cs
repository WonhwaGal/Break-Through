using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BaseGamePanel : MonoBehaviour
{
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _saveButton.onClick.AddListener(SaveGame);
        _exitButton.onClick.AddListener(Quit);
        if(SceneManager.GetActiveScene().buildIndex == 3)
            _saveButton.interactable = false;
    }

    private void SaveGame()
    {
        GameEventSystem.Send<SaveGameEvent>(new SaveGameEvent(true));
    }

    private void Quit() => Application.Quit();

    protected void BaseOnDestroy()
    {
        _saveButton.onClick.RemoveListener(SaveGame);
        _exitButton.onClick.RemoveListener(Quit);
    }
}