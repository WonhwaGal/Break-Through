using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameAudioScriptable), menuName = "SpawnSystem/GameAudio")]
public class GameAudioScriptable : ScriptableObject
{
    public AudioClip MenuBackground;
    public AudioClip NatureBackground;
    public AudioClip WaterfallSound;
    public AudioClip RiverSound;
}