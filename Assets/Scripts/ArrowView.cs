using UnityEngine;

public class ArrowView : MonoBehaviour
{
    [SerializeField] private Rigidbody _arrowRb;

    private const float Force = 10;
    public ArrowType ArrowType { get; set; }

    public void ShootArrow(Vector3 direction)
    {
        _arrowRb.AddForce(direction * Force, ForceMode.Impulse);
    }
}
