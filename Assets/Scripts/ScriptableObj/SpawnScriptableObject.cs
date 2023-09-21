using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SpawnScriptableObject), menuName = "SpawnSystem/SpawnScriptableObject")]
public class SpawnScriptableObject : ScriptableObject
{
    [field: SerializeField] public EnemyView EnemyPrefab; 
    [field: SerializeField] public GameObject ArrowPrefab; 
    [field: SerializeField] public SettingsMenuCanvas SettingsMenuPrefab; 
}
