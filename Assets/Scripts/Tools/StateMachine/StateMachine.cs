using System;

public sealed class StateMachine : IStateMachine
{
    public IState CurrentState { get; private set; }

    public void ChangeTo<TState>(Action<TState> setUpCallback) where TState : IState, new()
    {
        if (CurrentState != null && CurrentState.GetType().Equals(typeof(TState)))
            return;

        CurrentState?.ExitState();
        CurrentState?.Dispose();
        
        var state = Activator.CreateInstance<TState>();  //CurrentState = new T();
        setUpCallback?.Invoke(state);
        CurrentState = state;
        CurrentState.StateMachine = this;
        CurrentState.EnterState();
    }

    public void UpdateStateMachine() => CurrentState.UpdateState();
}