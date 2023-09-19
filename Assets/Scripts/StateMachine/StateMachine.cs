using System;

public class StateMachine : IStateMachine
{
    public IState CurrentState { get; private set; }

    public void ChangeTo<T>(Action<T> setUpCallback) where T : IState, new()
    {
        if (CurrentState != null && CurrentState.GetType().Equals(typeof(T)))
            return;

        CurrentState?.ExitState();
        CurrentState?.Dispose();
        
        var state = Activator.CreateInstance<T>();  //CurrentState = new T();
        setUpCallback?.Invoke(state);
        CurrentState = state;
        CurrentState.StateMachine = this;
        CurrentState.EnterState();
    }

    public void UpdateStateMachine() => CurrentState.UpdateState();
}