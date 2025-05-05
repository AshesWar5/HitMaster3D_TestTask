using Core.Infrastructures.GameFlow;
using Core.Services.SceneLoader;
using UnityEngine;
using Zenject;

namespace Core.Infrastructures.StateMachines.Game
{
    public sealed class LoadLevelState : State<GameStateMachine>, IPayLoadedState<string>
    {
        private readonly ISceneLoaderService _sceneLoader;

        public LoadLevelState(GameStateMachine stateMachine, ISceneLoaderService sceneLoader) :
            base(stateMachine) => _sceneLoader = sceneLoader;

        public async void Enter(string sceneName)
        {
            await _sceneLoader.Load(sceneName);
            var sceneContext = Object.FindAnyObjectByType<SceneContext>();
            var gameFlow = sceneContext.Container.Resolve<IGameFlow>();
            _stateMachine.Enter<GameLoopState, IGameFlow>(gameFlow);
        }

        public void Exit() { }
    }
}