namespace Core.Infrastructures.StateMachines
{
    public interface IStateFactory<TStateMachine>
    {
        TState CreateState<TState>(TStateMachine stateMachine) where TState : IExitableState;
    }
}