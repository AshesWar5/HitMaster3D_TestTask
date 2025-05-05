namespace Core.Infrastructures.StateMachines
{
    public interface IPayLoadedState<TPayload> : IExitableState
    {
        void Enter(TPayload parameter);
    }
}