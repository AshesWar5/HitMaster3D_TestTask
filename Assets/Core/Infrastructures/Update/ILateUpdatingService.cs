using Core.Infrastructures;

namespace Core.Infrastructures.Update
{
    public interface ILateUpdatingService : IUpdateService<ILateUpdating> { }
}