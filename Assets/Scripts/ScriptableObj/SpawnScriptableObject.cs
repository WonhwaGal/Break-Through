using UnityEngine;


[CreateAssetMenu(fileName = nameof(SpawnScriptableObject), menuName = "SpawnSystem/SpawnScriptableObject")]
public class SpawnScriptableObject : ScriptableObject
{
    public EnemyView EnemyPrefab; 
    public ArrowView ArrowPrefab; 
    public GameObject PointerPrefab; 
    public PlayerView PlayerPrefab;
    public EnemyView BossPrefab;
    public GameObject PlayerDefaultSpawn;
    public GameObject PlayerFinalSpawn;
}