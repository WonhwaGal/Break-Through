using System;
using UnityEngine;

public interface IInputService : IDisposable
{
    Vector3 KeyAxis { get; }
    Vector3 MouseAxis { get; }
}