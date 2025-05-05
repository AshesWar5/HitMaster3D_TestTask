using System;
using Core.Infrastructures;
using Core.Infrastructures.StateMachines.Game;
using Core.Logic.Battle;

namespace Core.Logic.Game
{
    public sealed class GamePageHandler : IInitialize, IDisposable
    {
        private readonly GamePageView _page;
        private readonly GameStateMachine _stateMachine;
        private readonly IBattleStart _battle;

        public GamePageHandler(GamePageView page, GameStateMachine stateMachine,
            IBattleStart battle)
        {
            _page = page;
            _stateMachine = stateMachine;
            _battle = battle;
        }

        public void Initialize()
        {
            _page.StartGameClick += OnStartGame;
            _page.RestartGameClick += OnRestartGame;
        }

        public void Dispose()
        {
            _page.StartGameClick -= OnStartGame;
            _page.RestartGameClick -= OnRestartGame;
        }

        private void OnStartGame()
        {
            _battle.StartBattle();
            _page.UpdateVisibilityButtonStartGame(false);
        }

        private void OnRestartGame()
        {
            _stateMachine.Dispose();
            _stateMachine.Enter<BootstrapState>();
        }
    }
}