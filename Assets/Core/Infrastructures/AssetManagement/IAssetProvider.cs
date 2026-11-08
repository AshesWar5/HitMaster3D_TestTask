using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Core.Infrastructures.AssetManagement
{
    public interface IAssetProvider
    {
        UniTask InitializeAsync();

        UniTask UpdateCatalogsAsync();

        UniTask<long> GetDownloadSizeAsync(IEnumerable<string> labels);

        UniTask DownloadAsync(IEnumerable<string> labels, IProgress<float> progress);

        TAsset Load<TAsset>(string path) where TAsset : Object;
    }
}
