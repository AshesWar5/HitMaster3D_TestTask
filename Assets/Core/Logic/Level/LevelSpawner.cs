using Core.Infrastructures.AssetManagement;
using Core.Logic.Factories;
using Core.Logic.Spawner;

namespace Core.Logic.Level
{
    public sealed class LevelSpawner : ObjectSpawner<LevelComponent>
    {
        private readonly ComponentReference<LevelComponent> _prefab;

        public LevelSpawner(IGameFactory gameFactory, ComponentReference<LevelComponent> prefab) :
            base(gameFactory) => _prefab = prefab;

        protected override LevelComponent CreateInternal() => _gameFactory.CreateLevel(_prefab);
    }
}
