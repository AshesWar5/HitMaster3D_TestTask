using Core.Logic.Bullet;
using Core.Logic.Factories;
using Core.Logic.Pool;
using Core.Services.Input;
using UnityEngine;
using Zenject;
using Component = Core.Infrastructures.Component;

namespace Core.Logic.Gun
{
    public class GunComponent : Component
    {
        [SerializeField] private Transform _pointSpawnBullet;
        [SerializeField] private int _initBulletsToPool = 10;
        
        private float _speedBullet;
        private BulletConfig _bulletConfig;
        private PoolQueue<BulletComponent> _bullets;
        private IInputService _input;
        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory, IInputService input)
        {
            _input = input;
            _gameFactory = gameFactory;
        }

        public void SetDataByBullet(float speed, BulletConfig config)
        {
            _speedBullet = speed;
            _bulletConfig = config;
        }

        protected override void OnInitialize()
        {
            _bullets = new PoolQueue<BulletComponent>();

            PrepareBullets();

            _input.ShootDirection += OnShootDirection;
        }

        protected override void OnDispose()
        {
            _input.ShootDirection -= OnShootDirection;
            
            foreach (BulletComponent bullet in _bullets.ListObjects)
            {
                _gameFactory.Remove(bullet);
            }
            
            _bullets.Clear();
        }

        private void PrepareBullets()
        {
            for (int i = 0; i < _initBulletsToPool; i++)
            {
                BulletComponent bullet = CreateBullet();
                _bullets.ReturnToPool(bullet);
            }
        }

        private BulletComponent CreateBullet()
        {
            var instance = _gameFactory.CreateBullet<BulletComponent>(_bulletConfig,
                _pointSpawnBullet.position);
            _bullets.Add(instance);

            return instance;
        }

        private void OnShootDirection(Vector3 direction)
        {
            if(!IsActiveComponent)
                return;
            
            BulletComponent bullet;
            
            if (_bullets.HasInactiveObjects())
            {
                bullet = _bullets.GetInactiveObject();
            }
            else
            {
                bullet = CreateBullet();
            }

            bullet.transform.position = _pointSpawnBullet.position;
            bullet.Move(direction, _speedBullet);
        }
    }
}