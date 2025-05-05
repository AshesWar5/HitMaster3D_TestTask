using System;
using Core.Infrastructures;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.Services.SceneLoader
{
    public interface ISceneLoaderService : IInitialize, IDisposable
    {
        event Action<Scene> LoadedScene;
        
        UniTask<bool> Load(string name);
    }
}