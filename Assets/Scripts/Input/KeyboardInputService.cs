using UnityEngine;

public sealed class KeyboardInputService : IInputService, IService
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    private const string MouseXAxis = "Mouse X";
    private const string MouseYAxis = "Mouse Y";

    private bool _isPaused;

    public KeyboardInputService()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameEventSystem.Subscribe<GameStopEvent>(UpdatePause);
    }

    public Vector3 KeyAxis => GetInputAxis();
    public Vector3 MouseAxis => GetMouseAxis();

    private Vector3 GetInputAxis()
    {
        CheckForPause();

        if (!_isPaused)
            return new(Input.GetAxis(Horizontal), 0, Input.GetAxis(Vertical));
        return Vector3.zero;
    }

    private Vector3 GetMouseAxis()
    {
        if (!_isPaused)
        {
            SendMouseEvents();
            return new(Input.GetAxis(MouseXAxis), Input.GetAxis(MouseYAxis), 0);
        }

        return Vector3.zero;
    }

    private async void UpdatePause(GameStopEvent @event)
    {
        if (_isPaused && !@event.IsPaused)
        {
            await System.Threading.Tasks.Task.Delay(50);
            _isPaused = @event.IsPaused;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (@event.EndOfGame)
        {
            _isPaused = @event.IsPaused;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void SendMouseEvents()
    {
        if (Input.GetMouseButtonDown(0))
            GameEventSystem.Send<PlayerAimEvent>(new PlayerAimEvent(true));

        if (Input.GetMouseButtonUp(0))
            GameEventSystem.Send<PlayerAimEvent>(new PlayerAimEvent(false));
    }

    private void CheckForPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isPaused)
        {
            GameEventSystem.Send<GameStopEvent>(new GameStopEvent(isPaused: true, false, false));
            _isPaused = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Dispose()
    {
        GameEventSystem.UnSubscribe<GameStopEvent>(UpdatePause);
    }
}