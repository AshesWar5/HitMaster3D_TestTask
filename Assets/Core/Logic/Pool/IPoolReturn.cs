namespace Core.Logic.Pool
{
    public interface IPoolReturn
    {
        void ReturnToPool(IPoolObject obj);
    }
}