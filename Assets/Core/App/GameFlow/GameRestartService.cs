using Core.App.StateMachines;
using Core.Infrastructures.GameFlow;
using Cysharp.Threading.Tasks;

namespace Core.App.GameFlow
{
    public sealed class GameRestartService : IGameRestart
    {
        private readonly GameStateMachine _gameStateMachine;

        private bool _isRestarting;

        public GameRestartService(GameStateMachine gameStateMachine) =>
            _gameStateMachine = gameStateMachine;

        public async void Restart()
        {
            if(_isRestarting)
                return;

            _isRestarting = true;

            await UniTask.Yield();

            _gameStateMachine.Dispose();
            _gameStateMachine.Enter<BootstrapState>();

            _isRestarting = false;
        }
    }
}
