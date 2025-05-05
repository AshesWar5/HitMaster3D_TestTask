using System;
using Core.Services.Page;
using UnityEngine;

namespace Core.Logic.Game
{
    public sealed class GamePageView : PageView
    {
        public event Action StartGameClick;
        public event Action RestartGameClick;

        [SerializeField] private ButtonView _startGame;
        [SerializeField] private ButtonView _restart;

        protected override void OnInitializeInternal()
        {
            _startGame.Click += OnStartGameClick;
            _restart.Click += OnRestartClick;
        }

        protected override void OnDisposeInternal()
        {
            _startGame.Click -= OnStartGameClick;
            _restart.Click -= OnRestartClick;
        }

        public void UpdateVisibilityButtonStartGame(bool visibility) =>
            _startGame.UpdateVisibility(visibility);

        private void OnStartGameClick() =>
            StartGameClick?.Invoke();

        private void OnRestartClick() =>
            RestartGameClick?.Invoke();
    }
}