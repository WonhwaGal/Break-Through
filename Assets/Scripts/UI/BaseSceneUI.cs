using UnityEngine;
using UnityEngine.UI;

public class BaseSceneUI : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] protected UIScriptableObject _prefabs;
    [SerializeField] private GameObject _settingsSpawn;

    private SettingsMenuCanvas _settingsCanvas;
    private Button _pauseSettingsButton;

    protected void ShowSettingsContainer(GameObject invokingPanel)
    {
        invokingPanel.SetActive(false);

        if (_settingsCanvas == null)
        {
            _settingsCanvas = Instantiate<SettingsMenuCanvas>(_prefabs.SettingsMenu, _settingsSpawn.transform);
            _settingsCanvas.transform.SetParent(this.transform);
            _settingsCanvas.transform.SetAsLastSibling();
            _settingsCanvas.ReturnButton.onClick.AddListener(() => HideSettings(invokingPanel, _settingsCanvas.gameObject));
            return;
        }

        _settingsCanvas.gameObject.SetActive(true);
    }

    public void ShowPanel(BaseGamePanel panel, bool shouldShow = true)
    {
        if (panel != null)
            panel.gameObject.SetActive(shouldShow);
    }

    public void AssignPausePanelButton(PausePanel pausePanel)
    {
        _pauseSettingsButton = pausePanel.SettingsButton;
        _pauseSettingsButton.onClick.AddListener(() => ShowSettingsContainer(pausePanel.gameObject));
    }

    private void HideSettings(GameObject invokingPanel, GameObject settingsPanel)
    {
        invokingPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        _settingsCanvas.ReturnButton.onClick.RemoveAllListeners();
        _pauseSettingsButton.onClick.RemoveAllListeners();
    }
}