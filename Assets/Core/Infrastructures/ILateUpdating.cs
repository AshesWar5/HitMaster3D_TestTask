using Core.Services.Update;

namespace Core.Infrastructures
{
    public interface ILateUpdating : IUpdate
    {
        void LateUpdating();
    }
}