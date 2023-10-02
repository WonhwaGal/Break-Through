using System;

public abstract class BaseStateOf<T> : IState
    where T : class
{
    private IStateMachine _stateMachine;
    private T _owner;

    public BaseStateOf() { }

    public IStateMachine StateMachine
    {
        get => _stateMachine;
        set
        {
            if (_stateMachine != null)
                return;
            _stateMachine = value;
        }
    }

    public T Owner
    {
        get => _owner;
        set
        {
            if (_owner != null)
                return;
            _owner = value;
        }
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void UpdateState() { }

    public void Dispose()
    {
        Owner = null;
        StateMachine = null;
        GC.SuppressFinalize(this);
    }
}