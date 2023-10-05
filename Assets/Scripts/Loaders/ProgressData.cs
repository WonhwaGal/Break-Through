using System;
using UnityEngine;

[Serializable]
public class ProgressData : IService
{
    public Vector3 PlayerPos;
    public int PlayerHP;
    public int ArrowsNumber;
    public int EnemiesKilledNumber;
    public int KeysNumber;
    public int MusicVolume;
    public int SoundVolume;
    public int MouseSensitivity;
}
