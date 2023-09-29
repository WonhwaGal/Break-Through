using UnityEngine;

[CreateAssetMenu(fileName = nameof(UIScriptableObject), menuName = "SpawnSystem/UIScriptableObject")]
public class UIScriptableObject : ScriptableObject
{
    public SettingsMenuCanvas SettingsMenu;
}