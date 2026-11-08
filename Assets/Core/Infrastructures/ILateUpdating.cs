using Core.Infrastructures.Update;

namespace Core.Infrastructures
{
    public interface ILateUpdating : IUpdate
    {
        void LateUpdating();
    }
}