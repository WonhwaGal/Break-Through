using UnityEngine;

public static class Constants
{
    [Header("General Settings :")]
    public const int StartArrowNumber = 15;
    public const int KillRewardMaxValue = 5;
    public const int ArrowDamageToPlayer = 20;
    public const int ArrowDamageToEnemy = 50;

    [Header("Enemy System :")]
    public static Vector3 RewardSpawnYShift = new Vector3(0, 7, 0);
}
