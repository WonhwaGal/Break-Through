using System;
using UnityEngine;

public class KeyboardInputService : IInputService
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    public const string MouseXAxis = "Mouse X";
    public const string MouseYAxis = "Mouse Y";

    public Vector3 KeyAxis => GetInputAxis();
    public Vector3 MouseAxis => GetMouseAxis();

    public event Action<bool> OnPressingAim;

    private Vector3 GetInputAxis() => new(Input.GetAxis(Horizontal), 0, Input.GetAxis(Vertical));
    private Vector3 GetMouseAxis()
    {
        if (Input.GetMouseButtonDown(0))
            OnPressingAim?.Invoke(true);
        if(Input.GetMouseButtonUp(0))
            OnPressingAim?.Invoke(false);

        return new(Input.GetAxis(MouseXAxis), Input.GetAxis(MouseYAxis), 0);
    }
}
