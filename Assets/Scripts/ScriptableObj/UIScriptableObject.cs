using UnityEngine;

[CreateAssetMenu(fileName = nameof(UIScriptableObject), menuName = "SpawnSystem/UIScriptableObject")]
public class UIScriptableObject : ScriptableObject
{
    public SettingsMenu SettingsMenu;
    public RewardIcon RewardIcon;
    public Sprite KeySprite;
    public Sprite ArrowSprite;
    public Sprite HpSprite;
}