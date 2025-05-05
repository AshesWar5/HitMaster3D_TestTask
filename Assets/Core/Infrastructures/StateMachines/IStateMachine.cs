using System;

namespace Core.Infrastructures.StateMachines
{
    public interface IStateMachine<T> : IDisposable where T : IStateMachine<T>
    {
        void Enter<TState>() where TState : State<T>, IState;
    }
}