using UnityEngine;

namespace Core.Infrastructures.AssetManagement
{
    public interface IAssetProvider
    {
        TAsset Load<TAsset>(string path) where TAsset : Object;
    }
}
