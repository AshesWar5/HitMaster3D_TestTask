using Core.Infrastructures.AssetManagement;
using Core.Infrastructures.StateMachines;
using Core.Services.ContentLoading;
using Cysharp.Threading.Tasks;

namespace Core.App.StateMachines
{
    public sealed class LoadContentState : State<GameStateMachine>, IPayLoadedState<string>
    {
        private readonly IContentLoadingService _contentLoading;

        private bool _isContentLoaded;

        public LoadContentState(GameStateMachine stateMachine, IContentLoadingService contentLoading) :
            base(stateMachine) => _contentLoading = contentLoading;

        public void Enter(string sceneName) =>
            EnterAsync(sceneName).Forget();

        public void Exit() { }

        private async UniTask EnterAsync(string sceneName)
        {
            if (!_isContentLoaded)
            {
                await _contentLoading.LoadAsync(AssetLabel.CORE, AssetLabel.UI);
                _isContentLoaded = true;
            }

            _stateMachine.Enter<LoadLevelState, string>(sceneName);
        }
    }
}
