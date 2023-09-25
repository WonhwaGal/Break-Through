using UnityEngine;
using UnityEngine.UI;

public abstract class SettingsPanel : MonoBehaviour
{
    public GameObject Panel;
    public Slider Slider;
    public Button ApplyButton;
    public Button DefaultButton;
    public Button ReturnButton;

    private void OnDestroy()
    {
        ApplyButton.onClick.RemoveAllListeners();
        DefaultButton.onClick.RemoveAllListeners();
        ReturnButton.onClick.RemoveAllListeners();
    }
}