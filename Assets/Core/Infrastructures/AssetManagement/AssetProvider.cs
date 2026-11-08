using UnityEngine;

namespace Core.Infrastructures.AssetManagement
{
    public sealed class AssetProvider : IAssetProvider
    {
        public TAsset Load<TAsset>(string path) where TAsset : Object =>
            Resources.Load<TAsset>(path);
    }
}
