using Cysharp.Threading.Tasks;

namespace Core.Services.ContentLoading
{
    public interface IContentLoadingService
    {
        UniTask LoadAsync(params string[] labels);
    }
}
