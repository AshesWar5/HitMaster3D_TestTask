using Core.Logic.Factories;
using Core.Logic.Spawner;
using UnityEngine;

namespace Core.Logic.Player
{
    public sealed class PlayerSpawner : ObjectSpawner<PlayerComponent>
    {
        private readonly PlayerConfig _playerConfig;
        private readonly Transform _point;

        public PlayerSpawner(IGameFactory gameFactory, PlayerConfig playerConfig,
            Transform point) : base(gameFactory)
        {
            _playerConfig = playerConfig;
            _point = point;
        }

        protected override PlayerComponent CreateInternal() =>
            _gameFactory.CreatePlayer(_point.position, _playerConfig);
    }
}