using UnityEngine;

[CreateAssetMenu(fileName = nameof(PlayerAudio), menuName = "SpawnSystem/PlayerAudio")]
public class PlayerAudio : ScriptableObject
{
    public AudioClip GroundFootstep;
    public AudioClip WaterFootStep;
    public AudioClip PlayerHurt;
    public AudioClip PlayerDie;
    public AudioClip ArrowShoot;
}