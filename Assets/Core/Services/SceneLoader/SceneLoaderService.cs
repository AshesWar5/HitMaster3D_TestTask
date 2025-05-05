using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Services.SceneLoader
{
    public sealed class SceneLoaderService : ISceneLoaderService
    {
        public event Action<Scene> LoadedScene;
        
        private bool _sceneIsLoading;

        public void Initialize() =>
            SceneManager.sceneLoaded += OnLoadedScene;

        public void Dispose() =>
            SceneManager.sceneLoaded -= OnLoadedScene;

        public async UniTask<bool> Load(string name)
        {
            if (_sceneIsLoading)
                return false;
            
            return await LoadScene(name);
        }

        private async UniTask<bool> LoadScene(string nextScene)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
                return true;

            _sceneIsLoading = true;
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);
            await waitNextScene;
            _sceneIsLoading = false;
            if (waitNextScene.isDone)
                return true;
            
            return false;
        }
        
        private void OnLoadedScene(Scene scene, LoadSceneMode mode) =>
            LoadedScene?.Invoke(scene);
    }
}