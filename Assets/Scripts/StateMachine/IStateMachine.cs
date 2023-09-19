using System;

public interface IStateMachine
{
    IState CurrentState { get; }
    void ChangeTo<T>(Action<T> setUpCallback) where T : IState, new();
    void UpdateStateMachine();
}
