using Core.Services.Update;

namespace Core.Infrastructures
{
    public interface IFixedUpdating : IUpdate
    {
        void FixedUpdating();
    }
}