using Core.Infrastructures.AssetManagement;
using Core.Infrastructures.StateMachines;
using Core.Services.ContentLoading;

namespace Core.App.StateMachines
{
    public sealed class LoadContentState : State<GameStateMachine>, IPayLoadedState<string>
    {
        private readonly IContentLoadingService _contentLoading;

        private bool _isContentLoaded;

        public LoadContentState(GameStateMachine stateMachine, IContentLoadingService contentLoading) :
            base(stateMachine) => _contentLoading = contentLoading;

        public async void Enter(string sceneName)
        {
            if (!_isContentLoaded)
            {
                await _contentLoading.LoadAsync(AssetLabel.CORE, AssetLabel.UI);
                _isContentLoaded = true;
            }

            _stateMachine.Enter<LoadLevelState, string>(sceneName);
        }

        public void Exit() { }
    }
}
