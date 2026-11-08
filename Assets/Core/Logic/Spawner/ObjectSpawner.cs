using Core.Infrastructures;
using Core.Logic.Factories;

namespace Core.Logic.Spawner
{
    public abstract class ObjectSpawner<TObject> where TObject : Component
    {
        protected readonly IGameFactory _gameFactory;

        protected TObject _currentObject;

        protected ObjectSpawner(IGameFactory gameFactory) =>
            _gameFactory = gameFactory;

        public TObject Create() =>
            _currentObject = CreateInternal();

        public void Remove() =>
            _gameFactory.Remove(_currentObject);

        protected abstract TObject CreateInternal();
    }
}