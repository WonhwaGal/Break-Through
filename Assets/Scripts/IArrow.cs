using UnityEngine;

public interface IArroww
{
    ArrowType ArrowType { get; set; }
    Rigidbody RigidBody { get; }
    float Force { get; }
}