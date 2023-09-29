using UnityEngine;

public interface IArrow
{
    ArrowType ArrowType { get; set; }
    Rigidbody RigidBody { get; }
    float Force { get; }
}