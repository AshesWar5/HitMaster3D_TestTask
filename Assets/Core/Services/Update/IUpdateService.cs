namespace Core.Services.Update
{
    public interface IUpdateService<T> where T : IUpdate
    {
        void Register(T update);
        
        void UnRegister(T update);
    }
}