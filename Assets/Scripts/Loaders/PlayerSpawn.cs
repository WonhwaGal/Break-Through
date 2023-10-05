using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Start()
    {
        Constants.PlayerDefaultSpawn = transform.position;
    }
}
