using Core.Services.Update;

namespace Core.Infrastructures
{
    public interface IUpdating : IUpdate
    {
        void Updating();
    }
}