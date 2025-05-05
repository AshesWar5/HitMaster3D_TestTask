namespace Core.Infrastructures.GameFlow
{
    public interface IGameFlow : IUpdating, IFixedUpdating
    {
        void StartGame();

        void StopGame();
    }
}