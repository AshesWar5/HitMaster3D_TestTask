using System;
using Core.Infrastructures.StateMachines;
using Core.Services.SceneLoader;

namespace Core.App.StateMachines
{
    public sealed class GameStateFactory : IStateFactory<GameStateMachine>
    {
        private readonly ISceneLoaderService _sceneLoader;

        public GameStateFactory(ISceneLoaderService sceneLoader) =>
            _sceneLoader = sceneLoader;

        public TState CreateState<TState>(GameStateMachine stateMachine) where TState : IExitableState
        {
            IExitableState state = typeof(TState) switch
            {
                Type type when type == typeof(BootstrapState) => new BootstrapState(stateMachine, _sceneLoader),
                Type type when type == typeof(LoadLevelState) => new LoadLevelState(stateMachine, _sceneLoader),
                Type type when type == typeof(GameLoopState) => new GameLoopState(stateMachine),
                _ => throw new ArgumentOutOfRangeException(nameof(TState), typeof(TState), null)
            };

            return (TState)state;
        }
    }
}
