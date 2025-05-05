namespace Core.Logic.Pool
{
    public interface IPoolObject
    {
        void AssignPool(IPoolReturn pool);
        void Enable();
        void Disable();
        void ReturnToPool();
        void ReleaseFromPool();
    }
}