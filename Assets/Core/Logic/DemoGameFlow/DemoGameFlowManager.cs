using System.Collections.Generic;
using Core.Infrastructures.AssetManagement;
using Core.Infrastructures.GameFlow;
using Core.Logic.Battle;
using Core.Logic.Camera;
using Core.Logic.Factories;
using Core.Logic.Game;
using Core.Logic.Level;
using Core.Logic.Player;
using Core.Services.ContentLoading;
using Core.Services.Page;
using Cysharp.Threading.Tasks;

namespace Core.Logic.DemoGameFlow
{
    public sealed class DemoGameFlowManager : GameFlowManager
    {
        private readonly CameraFollow _cameraFollow;
        private readonly IGameFactory _gameFactory;
        private readonly PlayerConfig _playerConfig;
        private readonly IPageService _pageService;
        private readonly IGameRestart _gameRestart;
        private readonly ILevelSelector _levelSelector;
        private readonly IContentLoadingService _contentLoading;
        private readonly IAssetProvider _assetProvider;

        private LevelSpawner _levelSpawner;
        private PlayerSpawner _playerSpawner;
        private BattlePreparation _battlePreparation;
        private GamePageHandler _gamePageHandler;
        private BattleManager _battleManager;
        private GamePageView _gamePageView;
        private LevelData _level;

        public DemoGameFlowManager(CameraFollow cameraFollow, IGameFactory gameFactory,
            PlayerConfig playerConfig, IPageService pageService, IGameRestart gameRestart,
            ILevelSelector levelSelector, IContentLoadingService contentLoading, IAssetProvider assetProvider)
        {
            _cameraFollow = cameraFollow;
            _gameFactory = gameFactory;
            _playerConfig = playerConfig;
            _pageService = pageService;
            _gameRestart = gameRestart;
            _levelSelector = levelSelector;
            _contentLoading = contentLoading;
            _assetProvider = assetProvider;
        }

        protected override async UniTask OnStartGameInternal()
        {
            _level = _levelSelector.Select();
            await _contentLoading.LoadAsync(_level.Label);

            _levelSpawner = new LevelSpawner(_gameFactory, _level.Prefab);
            LevelComponent level = _levelSpawner.Create();

            _playerSpawner = new PlayerSpawner(_gameFactory, _playerConfig, level.PlayerPoint);
            PlayerComponent player = _playerSpawner.Create();
            
            _cameraFollow.SetTarget(player.transform);

            _battlePreparation = new BattlePreparation(_gameFactory, level.BattlePlaces);
            IEnumerable<BattlePlaceData> battlePlaceDatas = _battlePreparation.Prepare();
            _battleManager = new BattleManager(player, battlePlaceDatas);
            _battleManager.Completed += OnBattleCompleted;
            _battleManager.Initialize();
            
            _pageService.Open(PageType.Game);
            _gamePageView = _pageService.Get<GamePageView>(PageType.Game);
            _gamePageHandler = new GamePageHandler(_gamePageView, _gameRestart, _battleManager);
            _gamePageHandler.Initialize();
        }

        protected override void OnStopGameInternal()
        {
            _gamePageHandler.Dispose();
            _battleManager.Completed -= OnBattleCompleted;
            _battleManager.Dispose();
            _battlePreparation.Clear();
            _playerSpawner.Remove();
            _levelSpawner.Remove();
            _pageService.Remove(_gamePageView);
            _assetProvider.Release(_level.Label);
        }

        private void OnBattleCompleted() =>
            _gameRestart.Restart();
    }
}