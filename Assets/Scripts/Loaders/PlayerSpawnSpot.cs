using UnityEngine;

public sealed class PlayerSpawnSpot : MonoBehaviour
{
    private void Start()
    {
        Constants.PlayerDefaultSpawn = transform.position;
    }
}
