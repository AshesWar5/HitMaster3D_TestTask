using Core.Infrastructures.Update;

namespace Core.Infrastructures
{
    public interface IUpdating : IUpdate
    {
        void Updating();
    }
}