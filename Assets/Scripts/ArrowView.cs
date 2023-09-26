using UnityEngine;

public class ArrowView : MonoBehaviour, IArrow
{
    [SerializeField] private Rigidbody _arrowRb;
    [SerializeField] private float _force = 10;

    public ArrowType ArrowType { get; set; }
    public Rigidbody RigidBody => _arrowRb;
    public float Force => _force;
}