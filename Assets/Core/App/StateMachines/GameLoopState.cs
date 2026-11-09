using Core.Infrastructures.GameFlow;
using Core.Infrastructures.StateMachines;
using Cysharp.Threading.Tasks;

namespace Core.App.StateMachines
{
    public sealed class GameLoopState : State<GameStateMachine>, IPayLoadedState<IGameFlow>
    {
        private IGameFlow _currentGameFlow;
        
        public GameLoopState(GameStateMachine stateMachine) : base(stateMachine) { }

        public void Enter(IGameFlow gameFlow)
        {
            _currentGameFlow = gameFlow;

            gameFlow.StartGame().Forget();
        }

        public void Exit() =>
            _currentGameFlow?.StopGame();

        protected override void OnDisposeInternal() =>
            _currentGameFlow?.StopGame();

        protected override void OnUpdatingInternal() =>
            _currentGameFlow?.Updating();
        
        protected override void OnFixedUpdatingInternal() =>
            _currentGameFlow?.FixedUpdating();
    }
}