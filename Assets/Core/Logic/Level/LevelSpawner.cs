using Core.Logic.Factories;
using Core.Logic.Player;
using Core.Logic.Spawner;

namespace Core.Logic.Level
{
    public sealed class LevelSpawner : ObjectSpawner<LevelComponent>
    {
        public LevelSpawner(IGameFactory gameFactory) : base(gameFactory) { }

        protected override LevelComponent CreateInternal() => _gameFactory.CreateLevel();
    }
}