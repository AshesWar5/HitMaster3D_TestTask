using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace Core.Infrastructures.AssetManagement
{
    public sealed class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, Dictionary<string, AsyncOperationHandle<Object>>> _preloadedAssets;

        public AssetProvider() =>
            _preloadedAssets = new Dictionary<string, Dictionary<string, AsyncOperationHandle<Object>>>();

        public async UniTask InitializeAsync()
        {
            var handle = Addressables.InitializeAsync(false);
            try
            {
                await Await(handle);
            }
            finally
            {
                Addressables.Release(handle);
            }
        }

        public async UniTask UpdateCatalogsAsync()
        {
            var handle = Addressables.CheckForCatalogUpdates(false);
            try
            {
                List<string> catalogs = await Await(handle);
                if(catalogs.Count == 0)
                    return;

                await UpdateCatalogsAsync(catalogs);
            }
            finally
            {
                Addressables.Release(handle);
            }
        }

        public async UniTask<long> GetDownloadSizeAsync(IEnumerable<string> labels)
        {
            var handle = LoadLocations(labels);
            try
            {
                IList<IResourceLocation> locations = await Await(handle);
                if(locations.Count == 0)
                    return 0;

                return await GetDownloadSizeAsync(locations);
            }
            finally
            {
                Addressables.Release(handle);
            }
        }

        public async UniTask DownloadAsync(IEnumerable<string> labels, IProgress<float> progress)
        {
            var handle = LoadLocations(labels);
            try
            {
                IList<IResourceLocation> locations = await Await(handle);
                if(locations.Count == 0)
                    return;

                await DownloadAsync(locations, progress);
            }
            finally
            {
                Addressables.Release(handle);
            }
        }

        public async UniTask PreloadAsync(string label)
        {
            if(_preloadedAssets.ContainsKey(label))
                return;

            var handle = Addressables.LoadResourceLocationsAsync(label);
            try
            {
                IList<IResourceLocation> locations = await Await(handle);
                Dictionary<string, AsyncOperationHandle<Object>> assets = await LoadAssetsAsync(locations);
                _preloadedAssets.Add(label, assets);
            }
            finally
            {
                Addressables.Release(handle);
            }
        }

        public TComponent Get<TComponent>(string address) where TComponent : MonoBehaviour =>
            GetPrefab(address).GetComponent<TComponent>();

        public TComponent Get<TComponent>(ComponentReference<TComponent> reference)
            where TComponent : MonoBehaviour =>
            GetPrefab(reference.RuntimeKey).GetComponent<TComponent>();

        public TAsset Load<TAsset>(string path) where TAsset : Object =>
            Resources.Load<TAsset>(path);

        public void Release(string label)
        {
            if(!_preloadedAssets.TryGetValue(label, out var assets))
                return;

            ReleaseAssets(assets);
            _preloadedAssets.Remove(label);
        }

        private async UniTask UpdateCatalogsAsync(List<string> catalogs)
        {
            var handle = Addressables.UpdateCatalogs(catalogs, false);
            try
            {
                await Await(handle);
            }
            finally
            {
                Addressables.Release(handle);
            }
        }

        private async UniTask<long> GetDownloadSizeAsync(IList<IResourceLocation> locations)
        {
            var handle = Addressables.GetDownloadSizeAsync(locations);
            try
            {
                return await Await(handle);
            }
            finally
            {
                Addressables.Release(handle);
            }
        }

        private async UniTask DownloadAsync(IList<IResourceLocation> locations, IProgress<float> progress)
        {
            var handle = Addressables.DownloadDependenciesAsync(locations);
            try
            {
                await Await(handle, progress);
            }
            finally
            {
                Addressables.Release(handle);
            }
        }

        private async UniTask<Dictionary<string, AsyncOperationHandle<Object>>> LoadAssetsAsync(
            IList<IResourceLocation> locations)
        {
            var assets = new Dictionary<string, AsyncOperationHandle<Object>>(locations.Count);
            for (int i = 0; i < locations.Count; i++)
            {
                IResourceLocation location = locations[i];
                if (!assets.ContainsKey(location.InternalId))
                {
                    assets.Add(location.InternalId, Addressables.LoadAssetAsync<Object>(location));
                }
            }

            try
            {
                foreach (AsyncOperationHandle<Object> asset in assets.Values)
                {
                    await Await(asset);
                }
            }
            catch (ContentLoadException)
            {
                ReleaseAssets(assets);
                throw;
            }

            return assets;
        }

        private GameObject GetPrefab(object key)
        {
            string internalId = Locate(key);
            foreach (Dictionary<string, AsyncOperationHandle<Object>> assets in _preloadedAssets.Values)
            {
                if (assets.TryGetValue(internalId, out var asset))
                {
                    return (GameObject)asset.Result;
                }
            }

            throw new ContentLoadException($"Asset '{key}' is not preloaded");
        }

        private string Locate(object key)
        {
            foreach (IResourceLocator locator in Addressables.ResourceLocators)
            {
                if (locator.Locate(key, typeof(GameObject), out IList<IResourceLocation> locations))
                {
                    return locations[0].InternalId;
                }
            }

            throw new ContentLoadException($"Asset '{key}' is not found");
        }

        private void ReleaseAssets(Dictionary<string, AsyncOperationHandle<Object>> assets)
        {
            foreach (AsyncOperationHandle<Object> asset in assets.Values)
            {
                Addressables.Release(asset);
            }
        }

        private AsyncOperationHandle<IList<IResourceLocation>> LoadLocations(IEnumerable<string> labels) =>
            Addressables.LoadResourceLocationsAsync(labels, Addressables.MergeMode.Union);

        private async UniTask<TResult> Await<TResult>(AsyncOperationHandle<TResult> handle)
        {
            try
            {
                return await handle.ToUniTask();
            }
            catch (Exception exception)
            {
                throw new ContentLoadException(exception.Message, exception);
            }
        }

        private async UniTask Await(AsyncOperationHandle handle, IProgress<float> progress)
        {
            try
            {
                await handle.ToUniTask(progress);
            }
            catch (Exception exception)
            {
                throw new ContentLoadException(exception.Message, exception);
            }
        }
    }
}
