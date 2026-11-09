using System;
using Core.Infrastructures.AssetManagement;
using Core.Infrastructures.Logger;
using Core.Services.Page;
using Core.Services.UI.Error;
using Core.Services.UI.Loading;
using Cysharp.Threading.Tasks;

namespace Core.Services.ContentLoading
{
    public sealed class ContentLoadingService : IContentLoadingService
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IPageService _pageService;
        private readonly ILoggerService _logger;
        private const int DOWNLOAD_TIMEOUT_SECONDS = 30;
        private const string STATUS_INITIALIZE = "Initialization...";
        private const string STATUS_DOWNLOAD = "Downloading content...";
        private const string STATUS_LOAD = "Loading...";
        private const string ERROR_MESSAGE = "Failed to load content.\nCheck your connection and try again.";

        private LoadingPageView _loadingPage;
        private ErrorPageView _errorPage;
        private UniTaskCompletionSource _retryCompletion;
        private bool _isInitialized;

        public ContentLoadingService(IAssetProvider assetProvider, IPageService pageService,
            ILoggerService logger)
        {
            _assetProvider = assetProvider;
            _pageService = pageService;
            _logger = logger;
        }

        public async UniTask LoadAsync(params string[] labels)
        {
            _pageService.Open(PageType.Loading);
            _loadingPage = _pageService.Get<LoadingPageView>(PageType.Loading);
            _loadingPage.SetProgress(0);

            if (!_isInitialized)
            {
                await Initialize();
            }

            await ExecuteWithRetry(() => Download(labels));
            await ExecuteWithRetry(() => Preload(labels));

            RemovePages();
        }

        private async UniTask Initialize()
        {
            _loadingPage.SetStatus(STATUS_INITIALIZE);

            await ExecuteWithRetry(_assetProvider.InitializeAsync);
            await UpdateCatalogs();

            _isInitialized = true;
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

        private async UniTask Download(string[] labels)
        {
            long size = await _assetProvider.GetDownloadSizeAsync(labels);
            if(size <= 0)
                return;

            _loadingPage.SetStatus(STATUS_DOWNLOAD);
            _loadingPage.SetProgress(0);

            await _assetProvider
                .DownloadAsync(labels, Progress.Create<float>(_loadingPage.SetProgress))
                .Timeout(TimeSpan.FromSeconds(DOWNLOAD_TIMEOUT_SECONDS));
        }

        private async UniTask Preload(string[] labels)
        {
            _loadingPage.SetStatus(STATUS_LOAD);

            for (int i = 0; i < labels.Length; i++)
            {
                _loadingPage.SetProgress((float)i / labels.Length);
                await _assetProvider.PreloadAsync(labels[i]);
            }

            _loadingPage.SetProgress(1);
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
            _pageService.Remove(_loadingPage);
            _loadingPage = null;

            if (_errorPage is not null)
            {
                _pageService.Remove(_errorPage);
                _errorPage = null;
            }
        }

        private void OnRetryClick() =>
            _retryCompletion.TrySetResult();
    }
}
