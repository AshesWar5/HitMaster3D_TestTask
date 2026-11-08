using System;
using Core.Infrastructures;
using Core.Infrastructures.GameFlow;
using Core.Logic.Battle;

namespace Core.Logic.Game
{
    public sealed class GamePageHandler : IInitialize, IDisposable
    {
        private readonly GamePageView _page;
        private readonly IGameRestart _gameRestart;
        private readonly IBattleStart _battle;

        public GamePageHandler(GamePageView page, IGameRestart gameRestart,
            IBattleStart battle)
        {
            _page = page;
            _gameRestart = gameRestart;
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

        private void OnRestartGame() =>
            _gameRestart.Restart();
    }
}