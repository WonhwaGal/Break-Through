using System;
using UnityEngine;

public interface IInputService
{
    Vector3 KeyAxis { get; }
    Vector3 MouseAxis { get; }

    event Action<bool> OnPressingAim;
    event Action OnPause;
}