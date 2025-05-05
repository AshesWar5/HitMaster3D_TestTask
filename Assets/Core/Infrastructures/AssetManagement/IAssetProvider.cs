using UnityEngine;

namespace Core.Infrastructures.AssetManagement
{
    public interface IAssetProvider
    {
        TType Instantiate<TType>(string path) where TType : MonoBehaviour;
        
        TType Instantiate<TType>(string path, Vector3 pos) where TType : MonoBehaviour;
        
        TType Instantiate<TType>(string path, Vector3 pos, Vector3 scale) where TType : MonoBehaviour;
        
        TType Instantiate<TType>(string path, Transform parent, Vector3 pos, Vector3 scale) where TType : MonoBehaviour;

        TType Instantiate<TType>(TType obj, Transform parent, Vector3 pos, Vector3 scale) where TType : MonoBehaviour;

        void Destroy(Object obj);
    }
}