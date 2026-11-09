using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Infrastructures.AssetManagement
{
    public interface IAssetProvider
    {
        UniTask InitializeAsync();

        UniTask UpdateCatalogsAsync();

        UniTask<long> GetDownloadSizeAsync(IEnumerable<string> labels);

        UniTask DownloadAsync(IEnumerable<string> labels, IProgress<float> progress);

        UniTask PreloadAsync(string label);

        TComponent Get<TComponent>(string address) where TComponent : MonoBehaviour;

        TComponent Get<TComponent>(ComponentReference<TComponent> reference) where TComponent : MonoBehaviour;

        TAsset Load<TAsset>(string path) where TAsset : Object;

        void Release(string label);
    }
}
