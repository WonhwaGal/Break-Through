using System;
using UnityEngine;

public class KeyboardInputService : IInputService, IService
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    public const string MouseXAxis = "Mouse X";
    public const string MouseYAxis = "Mouse Y";

    public Vector3 KeyAxis => GetInputAxis();
    public Vector3 MouseAxis => GetMouseAxis();

    public event Action OnPause;

    private Vector3 GetInputAxis()
    {
        if (Input.GetKey(KeyCode.Escape))
            OnPause?.Invoke();

        return new(Input.GetAxis(Horizontal), 0, Input.GetAxis(Vertical));
    }

    private Vector3 GetMouseAxis()
    {
        if (Input.GetMouseButtonDown(0))
            GameEventSystem.Send<PlayerAimEvent>(new PlayerAimEvent(true));

        if(Input.GetMouseButtonUp(0))
            GameEventSystem.Send<PlayerAimEvent>(new PlayerAimEvent(false));

        return new(Input.GetAxis(MouseXAxis), Input.GetAxis(MouseYAxis), 0);
    }
}