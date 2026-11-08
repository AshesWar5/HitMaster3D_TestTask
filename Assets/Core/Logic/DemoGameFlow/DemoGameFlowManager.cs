using System.Collections.Generic;
using Core.Infrastructures.Factories.Game;
using Core.Infrastructures.GameFlow;
using Core.Infrastructures.StateMachines.Game;
using Core.Logic.Battle;
using Core.Logic.Camera;
using Core.Logic.Game;
using Core.Logic.Level;
using Core.Logic.Player;
using Core.Services.Page;

namespace Core.Logic.DemoGameFlow
{
    public sealed class DemoGameFlowManager : GameFlowManager
    {
        private readonly CameraFollow _cameraFollow;
        private readonly IGameFactory _gameFactory;
        private readonly PlayerConfig _playerConfig;
        private readonly IPageService _pageService;
        private readonly GameStateMachine _gameStateMachine;

        private LevelSpawner _levelSpawner;
        private PlayerSpawner _playerSpawner;
        private BattlePreparation _battlePreparation;
        private GamePageHandler _gamePageHandler;
        private BattleManager _battleManager;
        private GamePageView _gamePageView;

        public DemoGameFlowManager(CameraFollow cameraFollow, IGameFactory gameFactory,
            PlayerConfig playerConfig, IPageService pageService, GameStateMachine gameStateMachine)
        {
            _cameraFollow = cameraFollow;
            _gameFactory = gameFactory;
            _playerConfig = playerConfig;
            _pageService = pageService;
            _gameStateMachine = gameStateMachine;
        }

        protected override void OnStartGameInternal()
        {
            _levelSpawner = new LevelSpawner(_gameFactory);
            LevelComponent level = _levelSpawner.Create();

            _playerSpawner = new PlayerSpawner(_gameFactory, _playerConfig, level.PlayerPoint);
            PlayerComponent player = _playerSpawner.Create();
            
            _cameraFollow.SetTarget(player.transform);

            _battlePreparation = new BattlePreparation(_gameFactory, level.BattlePlaces);
            IEnumerable<BattlePlaceData> battlePlaceDatas = _battlePreparation.Prepare();
            _battleManager = new BattleManager(player, battlePlaceDatas, _gameStateMachine);
            _battleManager.Initialize();
            
            _pageService.Open(PageType.Game);
            _gamePageView = _pageService.Get<GamePageView>(PageType.Game);
            _gamePageHandler = new GamePageHandler(_gamePageView, _gameStateMachine, _battleManager);
            _gamePageHandler.Initialize();
        }

        protected override void OnStopGameInternal()
        {
            _gamePageHandler.Dispose();
            _battleManager.Dispose();
            _battlePreparation.Clear();
            _playerSpawner.Remove();
            _levelSpawner.Remove();
            _pageService.Remove(_gamePageView);
        }
    }
}