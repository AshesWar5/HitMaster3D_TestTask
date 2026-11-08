using Core.Infrastructures.Update;

namespace Core.Infrastructures
{
    public interface IFixedUpdating : IUpdate
    {
        void FixedUpdating();
    }
}