using System;
using Core.Infrastructures.AssetManagement;
using Core.Infrastructures.Logger;
using Core.Infrastructures.StateMachines;
using Core.Services.Page;
using Core.Services.SceneLoader;

namespace Core.App.StateMachines
{
    public sealed class GameStateFactory : IStateFactory<GameStateMachine>
    {
        private readonly ISceneLoaderService _sceneLoader;
        private readonly IAssetProvider _assetProvider;
        private readonly IPageService _pageService;
        private readonly ILoggerService _logger;

        public GameStateFactory(ISceneLoaderService sceneLoader, IAssetProvider assetProvider,
            IPageService pageService, ILoggerService logger)
        {
            _sceneLoader = sceneLoader;
            _assetProvider = assetProvider;
            _pageService = pageService;
            _logger = logger;
        }

        public TState CreateState<TState>(GameStateMachine stateMachine) where TState : IExitableState
        {
            IExitableState state = typeof(TState) switch
            {
                Type type when type == typeof(BootstrapState) => new BootstrapState(stateMachine, _sceneLoader),
                Type type when type == typeof(LoadContentState) => new LoadContentState(stateMachine,
                    _assetProvider, _pageService, _logger),
                Type type when type == typeof(LoadLevelState) => new LoadLevelState(stateMachine, _sceneLoader),
                Type type when type == typeof(GameLoopState) => new GameLoopState(stateMachine),
                _ => throw new ArgumentOutOfRangeException(nameof(TState), typeof(TState), null)
            };

            return (TState)state;
        }
    }
}
