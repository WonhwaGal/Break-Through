using System;

public interface IState : IDisposable
{
    IStateMachine StateMachine { get; set; }
    void EnterState();
    void ExitState();
    void UpdateState();
}
