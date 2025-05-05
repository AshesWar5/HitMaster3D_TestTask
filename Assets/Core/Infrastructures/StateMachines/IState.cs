namespace Core.Infrastructures.StateMachines
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}