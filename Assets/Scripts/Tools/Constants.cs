using UnityEngine;

public static class Constants
{
    [Header("General Settings :")]
    public const int StartArrowNumber = 15;
    public const int UnlockFinalLevelKeyNumber = 3;
    public const int KillRewardMaxValue = 5;
    public const int ArrowDamageToPlayer = 20;
    public const int BossDamageToPlayer = 35;
    public const int ArrowDamageToEnemy = 50;

    [Header("Load Values :")]
    public const string SaveGameKey = "ProgressData";
    public const string ContinueSaved = "ContinueSaved";
    public static Vector3 PlayerDefaultSpawn = new (-59.93f, 0.12f, 297.41f);
    public static Vector3 PlayerFinalSpawn = new (37.22f, 8.3f, -13.0f);
    public const string CurrentArrowNumber = "CurrentArrows";

    [Header("UI Values :")]
    public const float MouseSensitivityStep = 0.25f;
    public const float DefaultMusic = -25;
    public const float DefaultSound = -10;
    public const float DefaultSens = 2;
    public const string PrefsSensitivity = "masterSensitivity";
    public const float DefaultAudioVolume = 0.5f;

    [Header("Audio Values :")]
    public const string PrefsMusic = "masterMusic";
    public const string PrefsSound = "masterSounds";
    public const string PrefsSens = "masterSensitivity";
    public const int MinAudioValue = -80;
    public const int MaxAudioValue = 20;

    [Header("Enemies :")]
    public static Vector3 RewardSpawnYShift = new (0, 7, 0);
}