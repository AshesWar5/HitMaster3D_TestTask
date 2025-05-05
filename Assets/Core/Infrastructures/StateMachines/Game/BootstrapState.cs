using Core.Services.SceneLoader;

namespace Core.Infrastructures.StateMachines.Game
{
    public sealed class BootstrapState : State<GameStateMachine>, IState
    {
        private readonly ISceneLoaderService _sceneLoader;
        private const string NEXT_NAME_SCENE = "Main";

        public BootstrapState(GameStateMachine stateMachine, ISceneLoaderService sceneLoader) :
            base(stateMachine) => _sceneLoader = sceneLoader;

        protected override void OnInitializeInternal() =>
            _sceneLoader.Initialize();

        protected override void OnDisposeInternal() =>
            _sceneLoader.Dispose();

        public void Enter() =>
            _stateMachine.Enter<LoadLevelState, string>(NEXT_NAME_SCENE);

        public void Exit() { }
    }
}