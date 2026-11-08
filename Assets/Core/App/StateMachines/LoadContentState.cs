using System;
using Core.Infrastructures.AssetManagement;
using Core.Infrastructures.Logger;
using Core.Infrastructures.StateMachines;
using Core.Services.Page;
using Core.Services.UI.Error;
using Core.Services.UI.Loading;
using Cysharp.Threading.Tasks;

namespace Core.App.StateMachines
{
    public sealed class LoadContentState : State<GameStateMachine>, IPayLoadedState<string>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IPageService _pageService;
        private readonly ILoggerService _logger;
        private readonly string[] _labels = { AssetLabel.CORE, AssetLabel.UI };
        private const int DOWNLOAD_TIMEOUT_SECONDS = 30;
        private const string STATUS_INITIALIZE = "Initialization...";
        private const string STATUS_DOWNLOAD = "Downloading content...";
        private const string ERROR_MESSAGE = "Failed to load content.\nCheck your connection and try again.";

        private LoadingPageView _loadingPage;
        private ErrorPageView _errorPage;
        private UniTaskCompletionSource _retryCompletion;
        private bool _isContentLoaded;

        public LoadContentState(GameStateMachine stateMachine, IAssetProvider assetProvider,
            IPageService pageService, ILoggerService logger) : base(stateMachine)
        {
            _assetProvider = assetProvider;
            _pageService = pageService;
            _logger = logger;
        }

        public async void Enter(string sceneName)
        {
            if(!_isContentLoaded)
                await LoadContent();

            _stateMachine.Enter<LoadLevelState, string>(sceneName);
        }

        public void Exit() { }

        private async UniTask LoadContent()
        {
            _pageService.Open(PageType.Loading);
            _loadingPage = _pageService.Get<LoadingPageView>(PageType.Loading);
            _loadingPage.SetStatus(STATUS_INITIALIZE);

            await ExecuteWithRetry(_assetProvider.InitializeAsync);
            await UpdateCatalogs();
            await ExecuteWithRetry(DownloadContent);

            _isContentLoaded = true;
            RemovePages();
        }

        private async UniTask UpdateCatalogs()
        {
            try
            {
                await _assetProvider.UpdateCatalogsAsync();
            }
            catch (ContentLoadException exception)
            {
                _logger.LogWarning(exception.Message);
            }
        }

        private async UniTask DownloadContent()
        {
            long size = await _assetProvider.GetDownloadSizeAsync(_labels);
            if(size <= 0)
                return;

            _loadingPage.SetStatus(STATUS_DOWNLOAD);
            _loadingPage.SetProgress(0);

            await _assetProvider
                .DownloadAsync(_labels, Progress.Create<float>(_loadingPage.SetProgress))
                .Timeout(TimeSpan.FromSeconds(DOWNLOAD_TIMEOUT_SECONDS));
        }

        private async UniTask ExecuteWithRetry(Func<UniTask> operation)
        {
            while (true)
            {
                try
                {
                    await operation();

                    return;
                }
                catch (Exception exception) when (exception is ContentLoadException or TimeoutException)
                {
                    _logger.LogError(exception.Message);
                }

                await WaitRetry();
            }
        }

        private async UniTask WaitRetry()
        {
            _pageService.Open(PageType.Error);
            _errorPage = _pageService.Get<ErrorPageView>(PageType.Error);
            _errorPage.SetMessage(ERROR_MESSAGE);
            _errorPage.RetryClick += OnRetryClick;

            _retryCompletion = new UniTaskCompletionSource();
            await _retryCompletion.Task;

            _errorPage.RetryClick -= OnRetryClick;
            _pageService.Close();
        }

        private void RemovePages()
        {
            _pageService.Close();
            _pageService.Remove(_loadingPage);

            if(_errorPage is not null)
                _pageService.Remove(_errorPage);
        }

        private void OnRetryClick() =>
            _retryCompletion.TrySetResult();
    }
}
