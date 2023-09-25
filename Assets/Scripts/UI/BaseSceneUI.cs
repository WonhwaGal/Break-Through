using UnityEngine;

public class BaseSceneUI : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private SpawnScriptableObject _prefabs;

    private SettingsMenuCanvas _settingsCanvas;
    
    protected void LoadSettingsContainer(GameObject invokingPanel)
    {
        invokingPanel.SetActive(false);

        if (_settingsCanvas != null)
            _settingsCanvas.gameObject.SetActive(true);

        _settingsCanvas = Instantiate<SettingsMenuCanvas>(_prefabs.SettingsMenuPrefab);
        _settingsCanvas.transform.SetParent(this.transform);
        _settingsCanvas.transform.SetAsLastSibling();
        _settingsCanvas.ReturnButton.onClick.AddListener(() => invokingPanel.SetActive(true));
    }
}