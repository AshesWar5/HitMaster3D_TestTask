using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace Core.Infrastructures.AssetManagement
{
    public sealed class AssetProvider : IAssetProvider
    {
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

        public TAsset Load<TAsset>(string path) where TAsset : Object =>
            Resources.Load<TAsset>(path);

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
