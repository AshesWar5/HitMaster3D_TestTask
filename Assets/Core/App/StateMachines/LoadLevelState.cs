using Core.Infrastructures.GameFlow;
using Core.Infrastructures.StateMachines;
using Core.Services.SceneLoader;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core.App.StateMachines
{
    public sealed class LoadLevelState : State<GameStateMachine>, IPayLoadedState<string>
    {
        private readonly ISceneLoaderService _sceneLoader;

        public LoadLevelState(GameStateMachine stateMachine, ISceneLoaderService sceneLoader) :
            base(stateMachine) => _sceneLoader = sceneLoader;

        public void Enter(string sceneName) =>
            EnterAsync(sceneName).Forget();

        public void Exit() { }

        private async UniTask EnterAsync(string sceneName)
        {
            await _sceneLoader.Load(sceneName);
            var sceneContext = Object.FindAnyObjectByType<SceneContext>();
            var gameFlow = sceneContext.Container.Resolve<IGameFlow>();
            _stateMachine.Enter<GameLoopState, IGameFlow>(gameFlow);
        }
    }
}