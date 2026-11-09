using Cysharp.Threading.Tasks;

namespace Core.Infrastructures.GameFlow
{
    public interface IGameFlow : IUpdating, IFixedUpdating
    {
        UniTask StartGame();

        void StopGame();
    }
}