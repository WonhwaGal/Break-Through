using UnityEngine;

public class KeyboardInputService : IInputService
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    public Vector2 Axis
    {
        get => GetInputAxis();
    }

    protected Vector2 GetInputAxis() =>
        new(Input.GetAxis(Horizontal), Input.GetAxis(Vertical));
}
