namespace Core.Infrastructures.StateMachines
{
    public interface IStateFactory
    {
        T CreateState<T>() where T : IExitableState;
    }
}