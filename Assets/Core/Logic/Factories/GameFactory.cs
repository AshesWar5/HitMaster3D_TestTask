using Core.Infrastructures.AssetManagement;
using Core.Infrastructures.Logger;
using Core.Infrastructures.Update;
using Core.Logic.Bullet;
using Core.Logic.Enemy;
using Core.Logic.Gun;
using Core.Logic.Level;
using Core.Logic.Player;
using Core.Services.UI;
using UnityEngine;
using Component = Core.Infrastructures.Component;

namespace Core.Logic.Factories
{
    public sealed class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IAssetInstantiator _assetInstantiator;
        private readonly IUpdatingService _updatingService;
        private readonly IFixedUpdatingService _fixedUpdatingService;
        private readonly IUIElementFactory _uiElementFactory;
        private readonly ILoggerService _logger;

        public GameFactory(IAssetProvider assetProvider, IAssetInstantiator assetInstantiator,
            IUpdatingService updatingService, IFixedUpdatingService fixedUpdatingService,
            IUIElementFactory uiElementFactory, ILoggerService logger)
        {
            _assetProvider = assetProvider;
            _assetInstantiator = assetInstantiator;
            _updatingService = updatingService;
            _fixedUpdatingService = fixedUpdatingService;
            _uiElementFactory = uiElementFactory;
            _logger = logger;
        }

        public PlayerComponent CreatePlayer(Vector2 pos, PlayerConfig config)
        {
            var scaleVector = new Vector3(config.Scale, config.Scale, config.Scale);
            var prefab = _assetProvider.Get(config.Prefab);
            var player = _assetInstantiator.Instantiate(prefab, null, pos, scaleVector);
            
            var stateMachine = new PlayerStateMachine(new PlayerStateFactory(), _logger, _updatingService,
                _fixedUpdatingService, player);

            var gun = CreateGun<GunComponent>(config.Gun, player.transform, player.PointSpawnGun.localPosition,
                player.PointSpawnGun.localEulerAngles);
            
            var animationHandler = new PlayerAnimationHandler(player.Animator);
            
            player.Setup(stateMachine, animationHandler, gun, config.Speed);
            player.Initialize();
            player.Enter();
            _updatingService.Register(player);
            _fixedUpdatingService.Register(player);

            return player;
        }

        public LevelComponent CreateLevel(ComponentReference<LevelComponent> prefab)
        {
            LevelComponent instance = _assetInstantiator.Instantiate(_assetProvider.Get(prefab), null,
                Vector3.zero, Vector3.one);
            instance.Initialize();
            instance.Enter();
            _updatingService.Register(instance);
            _fixedUpdatingService.Register(instance);

            return instance;
        }

        public TEnemy CreateEnemy<TEnemy>(EnemyConfig config, Vector3 position, int idBattlePlace)
            where TEnemy : EnemyComponent
        {
            Vector3 scale = new Vector3(config.Scale, config.Scale, config.Scale);
            var prefab = _assetProvider.Get(config.Prefab);
            var enemy = _assetInstantiator.Instantiate(prefab, null, position, scale).GetComponent<TEnemy>();
            
            var stateMachine = new EnemyStateMachine(new EnemyStateFactory(this), _logger, _updatingService,
                _fixedUpdatingService, enemy);
            
            var animationHandler = new EnemyAnimationHandler(enemy.Animator);

            var healthView = _uiElementFactory.Create<EnemyHealthView>(UIElementType.HealthEnemy,
                new Vector3(0, 2.3f, 0), enemy.transform);
            healthView.Enter();
            var healthHandler = new EnemyHealthHandler(config.Health, healthView);
            healthHandler.Initialize();
            
            enemy.Setup(stateMachine, animationHandler, healthHandler, idBattlePlace);
            enemy.Initialize();
            enemy.Enter();
            _updatingService.Register(enemy);
            _fixedUpdatingService.Register(enemy);

            return enemy;
        }
        
        public TGun CreateGun<TGun>(GunConfig config, Transform parent, Vector3 position, Vector3 rotation)
            where TGun : GunComponent
        {
            Vector3 scale = new Vector3(config.Scale, config.Scale, config.Scale);
            var prefab = _assetProvider.Get(config.Prefab);
            var gun = _assetInstantiator.Instantiate(prefab, parent, position, scale).GetComponent<TGun>();
            gun.transform.eulerAngles = rotation;
            
            gun.SetDataByBullet(config.SpeedBullet, config.BulletConfig);
            gun.Initialize();
            gun.Enter();
            _updatingService.Register(gun);
            _fixedUpdatingService.Register(gun);

            return gun;
        }
        
        public TBullet CreateBullet<TBullet>(BulletConfig config, Vector3 position)
            where TBullet : BulletComponent
        {
            Vector3 scale = new Vector3(config.Scale, config.Scale, config.Scale);
            var prefab = _assetProvider.Get(config.Prefab);
            var bullet = _assetInstantiator.Instantiate(prefab, null, position, scale).GetComponent<TBullet>();
            
            bullet.SetDamageValue(config.Damage);
            bullet.Initialize();
            bullet.Enter();
            _updatingService.Register(bullet);
            _fixedUpdatingService.Register(bullet);

            return bullet;
        }

        public void Remove(Component component)
        {
            if(component is null)
                return;

            _updatingService.UnRegister(component);
            _fixedUpdatingService.UnRegister(component);

            if(component == null)
                return;

            component.Dispose();
            _assetInstantiator.Destroy(component.gameObject);
        }
    }
}